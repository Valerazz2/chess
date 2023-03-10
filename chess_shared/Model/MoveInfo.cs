using Model;

namespace Chess.Model
{
    public class MoveInfo
    {
        public MoveType MoveType;
        public ChessColor MoveColor;
        public Square MovedFrom;
        public Piece Piece;
        public PieceClone CapturedPiece;
    }

    
}