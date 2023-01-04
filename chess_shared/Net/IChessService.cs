using chess_shared.Net;
using Net;

namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join(JoinArgs args);

        MoveResult MovePiece(MovePieceArgs args);

        AskNewsResult AskNews(AskNewsArgs args);
    }
}
