using Chess.Model;
using Common.Util.Undo;

namespace Model
{
    public class DeskHistory
    {
        private Desk _desk;
        public UndoManager undoManager = new();
        public DeskHistory(Desk desk)
        {
            _desk = desk;
            _desk.OnServerMove += AddCommandMove;
        }

        private void AddCommandMove(MoveInfo moveInfo)
        {
            CommandMove commandMove = new()
            {
                MovedFrom = moveInfo.MovedFrom,
                MovedTo = moveInfo.Piece.Square,
                CapturedPiece = moveInfo.CapturedPiece
            };
            
            undoManager.AddCommand(commandMove, false);
        }
    }
}