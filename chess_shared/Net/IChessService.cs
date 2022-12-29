using chess_shared.Net;

namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join();

        void MovePiece(MovePieceArgs args);

        AskNewsResult AskNews(AskNewsArgs args);
    }
}
