using System;
using System.Linq;
using chess_shared.Model;
using Newtonsoft.Json;


namespace Chess.Model
{
    public abstract class Piece : DeskObj
    {
        public bool WasMoved;
        public ChessColor Color;
        public Holder<Square> SquareHolder = new();

        public Square Square
        {
            get => SquareHolder.Value;
            set => SquareHolder.Value = value;
        }

        protected Piece(Desk getDesk) : base(getDesk) {}

        public bool TryMoveSuccess(Square target)
        {
            SquareHolder.EventsCall = false;
            var targetPiece = target.Piece;
            var oldSquare = Square;
        
            MoveToWithOutChecking(target);

            var king = Desk.FindKing(Color);
            var isCheck = Desk.IsCheckTo(king);
        
            MoveToWithOutChecking(oldSquare);
            
            target.Piece = targetPiece;
            SquareHolder.EventsCall = true;
            return !isCheck;
        }

        public void MoveToWithOutChecking(Square target)
        {
            SquareHolder.Value.Piece = null;
            target.Piece = this;
            SquareHolder.Value = target;
        }
        public void MoveTo(Square target)
        {
            if (AbleMoveTo(target) && TryMoveSuccess(target))
            {
                WasMoved = true;
                SquareHolder.Value.Piece = null;
                target.Piece = this;
                SquareHolder.Value = target;
                return;
            }
            throw new Exception("Loshara");
        }
        public abstract PieceType GetPieceType();
        public abstract bool AbleMoveTo(Square target);
        
        protected bool CheckTile(Square square, ChessColor chessColor)
        {
            return square.Piece == null || square.Piece.Color != chessColor;
        }

        public bool AbleMoveAnyWhere()
        {
            foreach (var square in Desk.ISquares)
            {
                if (AbleMoveTo(square) && TryMoveSuccess(square))
                {
                    return true;
                }
            }
            return Desk.ISquares.Any(square => AbleMoveTo(square) && TryMoveSuccess(square));
        }

        protected bool CheckTiles(Square target)
        {
            var step = SquareHolder.Value.Pos.GetStep(target.Pos);
            for (var pos = SquareHolder.Value.Pos + step; pos != target.Pos; pos += step)
            {
                if (Desk.GetPieceAt(pos) != null)
                {
                    return false;
                }
            }
            return CheckTile(target, Color);
        }
        public bool ReachedLastSquare()
        {
            return (SquareHolder.Value.Pos.Y == 0 || SquareHolder.Value.Pos.Y == Desk.DeskSizeY - 1) && GetPieceType() == PieceType.Pawn ;
        }
    }
}
