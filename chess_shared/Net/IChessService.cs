namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join();

        void SelectSquare(SelectSquareArgs args);

        AskNewsResult AskNews(AskNewsArgs newsArgs);
    }
}
