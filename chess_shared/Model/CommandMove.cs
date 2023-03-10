using System;
using Chess.Model;
using Common.Util.Undo;

namespace Model
{
    public class CommandMove : IUndoableCommand
    {
        public MoveType MoveType;
        public Square MovedFrom;
        public Square MovedTo;
        public PieceClone CapturedPiece;
        private Desk desk => MovedFrom.Desk;
        public void Apply()
        {
            switch (MoveType)
            {
                case MoveType.Castle:
                    break;
                case MoveType.Queening:
                    break;
                case MoveType.TakeOnPass:
                    break;
                case MoveType.DefaultMove:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            switch (MoveType)
            {
                
                case MoveType.Castle:
                    var deltaX = (MovedTo.Pos.X - MovedFrom.Pos.X) / 2;
                    var rook = desk.GetPieceAt(MovedTo.Pos - new Vector2Int(deltaX, 0));
                    piece.MoveToWithOutChecking(MovedFrom);
                    var posX = rook.Square.Pos.X <= 3 ? 0 : 7;
                    var square = desk.GetSquareAt(new Vector2Int(posX, rook.Square.Pos.Y));
                    rook.MoveToWithOutChecking(square);
                    break;
                case MoveType.Queening:
                    var backDir = piece.Square.Pos.Y == 7 ? -1 : 1;
                    desk.AddPiece(piece.Color, PieceType.Pawn, MovedFrom);
                    desk.Pieces.Remove(piece);
                    if (CapturedPiece != null)
                    {
                        desk.AddPiece(CapturedPiece.Color, CapturedPiece.PieceType, MovedTo);
                    }
                    break;
                case MoveType.TakeOnPass:
                    piece.MoveToWithOutChecking(MovedFrom);
                    var deltaY = MovedFrom.Pos.X - MovedTo.Pos.X;
                    var square2 = desk.GetSquareAt(MovedTo.Pos + new Vector2Int(0, deltaY));
                    desk.AddPiece(piece.Color.Invert(), PieceType.Pawn, square2);
                    break;
                case MoveType.DefaultMove:
                    piece.MoveToWithOutChecking(MovedFrom);
                    if (CapturedPiece != null)
                    {
                        desk.AddPiece(CapturedPiece.Color, CapturedPiece.PieceType, MovedTo);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}