using System;
using System.Collections.Generic;
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
        
        public readonly string CurrentSid;
        public event Action EnemyJoined;

        private List<string> appliedNewsId = new();
        public ChessNetClient(Desk desk, string playerSid)
        {
            CurrentSid = playerSid;
            this.desk = desk;
            desk.OnServerMove += OnMove;
        }

        private async void OnMove(MoveInfo moveInfo)
        {
            if (Color == moveInfo.MoveColor)
            {
                var moveResult = await httpClient.MovePiece(new MovePieceArgs
                {
                    Sid = CurrentSid,
                    MovedFrom = moveInfo.MovedFrom.GetRef(),
                    MovedTo = moveInfo.Piece.Square.GetRef(),
                });
            }
        }
        public async Task Join(JoinArgs joinArgs)
        {
            joinResult = await httpClient.Join(joinArgs);
        }

        public async Task CheckNews()
        {
            var news = await httpClient.AskNews(new AskNewsArgs
            {
                NewsID = appliedNewsId,
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
                    appliedNewsId.Add(New.ID);
                }
            }
        }

        public Desk GetDeskFor(string id)
        {
            return null;// httpClient.GetDeskFor(id);
        }
    }
}