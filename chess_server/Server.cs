using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Chess.Server;

namespace Chess.Model
{
    public class Server : IChessService
    {
        private static Server _inst;

        public static Server Instance
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new Server();
                }

                return _inst;
            }
        }

        public Dictionary<string, ServerPlayer> dictionary = new Dictionary<string, ServerPlayer>();
        
        private ServerPlayer waitingPlayer;
        
        readonly object joinLock = new object();

        public int GameCount => dictionary.Count / 2;

        private Server()
        {
            
        }

        public JoinResult Join()
        {
            lock (joinLock)
            {
                var game = waitingPlayer == null ? new ChessGame() : waitingPlayer.game;
                var color = waitingPlayer == null ? ChessColor.White : ChessColor.Black;
                var player = new ServerPlayer(game, color);
                dictionary.Add(player.ID, player);
                if (waitingPlayer == null)
                {
                    waitingPlayer = player;
                    game.PlayerWhite = player;
                }
                else
                {
                    waitingPlayer = null;
                    game.PlayerBlack = player;
                }

                return new JoinResult
                {
                    Sid = player.ID,
                    Color = player.Color
                };
            }
        }

        public void SelectSquare(SelectSquareArgs args)
        {
            ServerPlayer player;
            lock (dictionary)
            {
                if (!dictionary.TryGetValue(args.Sid, out player))
                {
                    throw new Exception("Player not found");
                }
            }

            var desk = player.game.Desk;
            lock (desk)
            {
                if (desk.move == player.Color)
                {
                    desk.Select(args.SquareRef);
                }
            }
        }
    }
}
