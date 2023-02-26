using chess_shared.Net;
using Chess.Server;
using Net;

namespace Chess.Model
{
    public class Server : IChessService
    {
        private static Server? _inst;

        public static Server Instance => _inst ??= new Server();

        private readonly Dictionary<string, ServerPlayer> playersId = new();
        
        private ServerPlayer? _waitingPlayer;
        
        readonly object _joinLock = new();

        public int GameCount => GetGameCount();

        private int GetGameCount()
        {
            lock (playersId)
            {
                return playersId.Count / 2;
            }
        }

        private Server()
        {
        }
        public JoinResult Join(JoinArgs args)
        {
            switch (args.GameMode)
            {
                case GameMode.RealEnemy:
                    lock (_joinLock)
                    {
                        var game = _waitingPlayer == null ? new ChessGame() : _waitingPlayer.game;
                        var color = _waitingPlayer == null ? ChessColor.White : ChessColor.Black;
                        var player = new ServerPlayer(game, color);
                        lock (playersId)
                        {
                            playersId.Add(args.PlayerId, player);
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

                        if (player.Color == ChessColor.Black)
                        {
                            player.game.PlayerWhite?.NewsForClient.Add(new EnemyJoined());
                        }
                        return new JoinResult
                        {
                            Color = player.Color
                        };
                    }
                case GameMode.ServerBot:
                    var gameWBot = new ChessGame();
                    var playerColor = ChessColor.White;
                    var realPlayer = new ServerPlayer(gameWBot, playerColor);
                    lock (playersId)
                    {
                        playersId.Add(args.PlayerId, realPlayer);
                    }

                    var chessServerBot = new ChessServerBot(gameWBot, ChessColor.Black);
                    gameWBot.PlayerWhite = realPlayer;
                    realPlayer.game.PlayerWhite?.NewsForClient.Add(new EnemyJoined());
                    return new JoinResult
                    {
                        Color = realPlayer.Color
                    };
            }
            return new JoinResult();
        }

        public MoveResult MovePiece(MovePieceArgs args)
        {
            var player = GetPlayer(args);
            var desk = player.game.Desk;
            lock (desk)
            {
                desk.Select(args.MovedFrom, player.Color);
                desk.Select(args.MovedTo, player.Color);
            }

            return new MoveResult();
        }

        public AskNewsResult AskNews(AskNewsArgs args)
        {
            var player = GetPlayer(args);
            
            var result = new AskNewsResult
            {
                News = new List<News>(player.NewsForClient)
            };
            return result;
        }

        public ServerPlayer? FindPlayer(string sid)
        {
            ServerPlayer? player;
            lock (playersId)
            {
                playersId.TryGetValue(sid, out player);
            }

            return player;
        }

        public ServerPlayer GetPlayer(AbstractSidArgs args)
        {
            return GetPlayer(args.Sid);
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
                lock (playersId)
                {
                    playersId.Clear();
                }
            
                _waitingPlayer = null;    
            }
        }
    }
}
