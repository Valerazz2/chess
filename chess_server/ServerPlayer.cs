using System;
using Chess.Server;
using Net;

namespace Chess.Model
{
    public class ServerPlayer
    {
        public readonly List<News> NewsForClient = new();
        
        public readonly ChessColor Color;
        
        public readonly string ID = Guid.NewGuid().ToString();

        public readonly ChessGame game;

        public ServerPlayer(ChessGame game, ChessColor color)
        {
            this.game = game;
            Color = color;
            this.game.Desk.OnServerMove += AddMoveNew;
        }

        private void AddMoveNew(MoveInfo moveInfo)
        {
            if (moveInfo.Piece.Color != Color)
            {
                var news = new EnemyFigureMoved
                {
                    MovedFrom = moveInfo.MovedFrom.GetRef(),
                    MovedTo = moveInfo.Piece.Square.GetRef()
                };
                NewsForClient.Add(news);
            }
        }
        
    }
}