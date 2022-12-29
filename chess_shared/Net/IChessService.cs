namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join();

        void MovePiece(OnMoveArgs args);

        AskNewsResult AskNews(AskNewsArgs newsArgs);
    }
}
