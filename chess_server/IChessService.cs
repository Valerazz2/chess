namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join();

        void SelectSquare(SelectSquareArgs? args);
    }
}
