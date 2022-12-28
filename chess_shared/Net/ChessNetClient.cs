using System;
using System.Threading;
using System.Threading.Tasks;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;

namespace Net
{
    public class ChessNetClient
    {
        public Desk desk;

        public ChessHttpClient httpClient = new();
        
        public JoinResult joinResult;
        
        public ChessColor Color => joinResult.Color;
        
        public string CurrentSid => joinResult.Sid;

        public bool IsMyMove => joinResult != null && Color == desk.move;

        public ChessNetClient(Desk desk)
        {
            this.desk = desk;
            desk.OnSelect += async square =>
            {
                if (!IsMyMove)
                {
                    return;
                }
                await httpClient.SelectSquare(new SelectSquareArgs
                {
                    Sid = CurrentSid,
                    SquareRef = square.GetRef()
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

        public void CheckNews()
        {
            var news = httpClient.AskNews(new AskNewsArgs
            {
                PlayerSid = CurrentSid
            });
            if (news.Result.News.Count > 0)
            {
                foreach (var New in news.Result.News)
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