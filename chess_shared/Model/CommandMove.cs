using Chess.Model;
using Common.Util.Undo;

namespace Model
{
    public class CommandMove : IUndoableCommand
    {
        public Square MovedFrom;
        public Square MovedTo;
        public PieceClone CapturedPiece;
        private Desk desk => MovedFrom.Desk;
        public void Apply()
        {
            var piece = MovedFrom.Piece;
            if (MovedTo.Piece != null)
            {
                desk.Pieces.Remove(MovedTo.Piece);
            }
            piece.MoveToWithOutChecking(MovedTo);
        }

        public void Revert()
        {
            var piece = MovedTo.Piece;
            piece.MoveToWithOutChecking(MovedFrom);
            if (CapturedPiece != null)
            {
                desk.AddPiece(CapturedPiece.Color, CapturedPiece.PieceType, MovedTo);
            }
        }
    }
}