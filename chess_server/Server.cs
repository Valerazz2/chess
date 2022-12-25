using Chess.Server;

namespace Chess.Model
{
    public class Server : IChessService
    {
        private static Server? _inst;

        public static Server Instance => _inst ??= new Server();

        private readonly Dictionary<string, ServerPlayer> _dictionary = new();
        
        private ServerPlayer? _waitingPlayer;
        
        readonly object _joinLock = new();

        public int GameCount => GetGameCount();

        private int GetGameCount()
        {
            lock (_dictionary)
            {
                return _dictionary.Count / 2;
            }
        }

        private Server()
        {
        }

        public JoinResult Join()
        {
            lock (_joinLock)
            {
                var game = _waitingPlayer == null ? new ChessGame() : _waitingPlayer.game;
                var color = _waitingPlayer == null ? ChessColor.White : ChessColor.Black;
                var player = new ServerPlayer(game, color);
                lock (_dictionary)
                {
                    _dictionary.Add(player.ID, player);
                }
                if (_waitingPlayer == null)
                {
                    _waitingPlayer = player;
                    game.PlayerWhite = player;
                }
                else
                {
                    _waitingPlayer = null;
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
            var player = GetPlayer(args.Sid);
            var desk = player.game.Desk;
            lock (desk)
            {
                if (desk.move == player.Color)
                {
                    desk.Select(args.SquareRef);
                }
            }
        }

        public ServerPlayer? FindPlayer(string sid)
        {
            ServerPlayer? player;
            lock (_dictionary)
            {
                _dictionary.TryGetValue(sid, out player);
            }

            return player;
        }
        
        public ServerPlayer GetPlayer(string sid)
        {
            var result = FindPlayer(sid);
            if (result == null)
            {
                throw new Exception("Player not found");
            }

            return result;
        }

        public void Clear()
        {
            lock (_joinLock)
            {
                lock (_dictionary)
                {
                    _dictionary.Clear();
                }
            
                _waitingPlayer = null;    
            }
        }
    }
}
