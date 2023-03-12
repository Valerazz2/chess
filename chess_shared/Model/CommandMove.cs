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
            var piece = MovedFrom.Piece;
            switch (MoveType)
            {
                case MoveType.Castle:
                    var rook = desk.FindFirstFigureByStep(MovedTo, piece);
                    desk.MoveRookWhenCastling(rook, piece);
                    piece.MoveToWithOutChecking(MovedTo);
                        break;
                case MoveType.Queening:
                    if (MovedTo.Piece != null)
                    {
                        desk.RemovePiece(MovedTo.Piece);
                    }
                    desk.AddPiece(piece.Color, PieceType.Queen, MovedTo);
                    desk.Pieces.Remove(piece);
                    break;
                case MoveType.TakeOnPass:
                    piece.MoveToWithOutChecking(MovedTo);
                    var deltaX = MovedTo.Pos.X - MovedFrom.Pos.X;
                    var capturedPiece = desk.GetPieceAt(MovedFrom.Pos + new Vector2Int(deltaX, 0));
                    desk.RemovePiece(capturedPiece);
                    break;
                case MoveType.DefaultMove:
                    if (MovedTo.Piece != null)
                    {
                        desk.RemovePiece(MovedTo.Piece);
                    }
                    piece.MoveToWithOutChecking(MovedTo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Revert()
        {
            var piece = MovedTo.Piece;
            switch (MoveType)
            {
                
                case MoveType.Castle:
                    var deltaX = Math.Sign(MovedTo.Pos.X - MovedFrom.Pos.X);
                    var rook = desk.GetPieceAt(MovedTo.Pos - new Vector2Int(deltaX, 0));
                    piece.MoveToWithOutChecking(MovedFrom);
                    var posX = rook.Square.Pos.X <= 3 ? 0 : 7;
                    var square = desk.GetSquareAt(new Vector2Int(posX, rook.Square.Pos.Y));
                    rook.MoveToWithOutChecking(square);
                    break;
                case MoveType.Queening:
                    desk.AddPiece(piece.Color, PieceType.Pawn, MovedFrom);
                    desk.RemovePiece(piece);
                    if (CapturedPiece != null)
                    {
                        desk.AddPiece(CapturedPiece.Color, CapturedPiece.PieceType, MovedTo);
                    }
                    break;
                case MoveType.TakeOnPass:
                    piece.MoveToWithOutChecking(MovedFrom);
                    var dirX = MovedTo.Pos.X - MovedFrom.Pos.X;
                    var square2 = desk.GetSquareAt(MovedFrom.Pos + new Vector2Int(dirX, 0));
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