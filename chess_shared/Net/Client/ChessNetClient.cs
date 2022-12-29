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

        public ChessNetClient(Desk desk)
        {
            this.desk = desk;
            desk.OnMove += async moveInfo =>
            {
                if (Color != moveInfo.MoveColor)
                {
                    return;
                }
                await httpClient.OnMove(new MovePieceArgs
                {
                    Sid = CurrentSid,
                    MovedFrom = moveInfo.MovedFrom.GetRef(),
                    MovedTo = moveInfo.Piece.Square.GetRef(),
                });
            };
        }
        
        public async Task<AskNewsResult> GetNews(AskNewsArgs args)
        {
            return await httpClient.AskNews(args);
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
                        case EnemyFigureMoved efm:
                            var enemyColor = Color.Invert();
                            var movedFrom = desk.GetSquareAt(efm.MovedFrom);
                            var movedTo = desk.GetSquareAt(efm.MovedTo);
                            desk.Select(movedFrom, enemyColor);
                            desk.Select(movedTo, enemyColor);
                            break;
                    }
                }
            }
        }
    }
}