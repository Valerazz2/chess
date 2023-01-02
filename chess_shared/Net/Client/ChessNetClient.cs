using System;
using System.Threading.Tasks;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;

namespace Net
{
    public class ChessNetClient
    {
        private Desk desk;

        public ChessHttpClient httpClient = new();
        
        public JoinResult joinResult;
        
        public ChessColor Color => joinResult.Color;
        
        public string CurrentSid => joinResult.Sid;
        public event Action EnemyJoined;

        public ChessNetClient(Desk desk)
        {
            this.desk = desk;
            desk.OnMove += OnMove;
        }

        private async void OnMove(MoveInfo moveInfo)
        {
            if (Color != moveInfo.MoveColor)
            {
                return;
            }

            while (true)
            {
                var success = await httpClient.OnMove(new MovePieceArgs
                {
                    Sid = CurrentSid,
                    MovedFrom = moveInfo.MovedFrom.GetRef(),
                    MovedTo = moveInfo.Piece.Square.GetRef(),
                });
                if (success)
                {
                    break;
                }
            }
        }
        public async Task Join()
        {
            joinResult = await httpClient.Join();
        }

        public async Task CheckNews()
        {
            var news = await httpClient.AskNews(new AskNewsArgs
            {
                Sid = CurrentSid
            });
            if (news.News.Count > 0)
            {
                foreach (var New in news.News)
                {
                    switch (New)
                    {
                        case EnemyFigureMoved figureMoved:
                            var enemyColor = Color.Invert();
                            var movedFrom = desk.GetSquareAt(figureMoved.MovedFrom);
                            var movedTo = desk.GetSquareAt(figureMoved.MovedTo);
                            desk.Select(movedFrom, enemyColor);
                            desk.Select(movedTo, enemyColor);
                            break;
                        
                        case EnemyJoined enemyJoined:
                            EnemyJoined?.Invoke();
                            break;
                    }
                }
            }
        }
    }
}