using System;

namespace Chess.Model
{
    public class PieceClone
    {
        public PieceType PieceType;
        private int count;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                CountChanged?.Invoke();
            }
        }
        
        public ChessColor Color;
        
        public event Action CountChanged;
        
        public PieceClone(ChessColor color, Desk desk, PieceType pieceType)
        {
            Color = color;
            PieceType = pieceType;
            desk.OnPieceCaptured += piece =>
            {
                if (piece.GetPieceType() == PieceType && piece.Color == Color)
                {
                    Count++;
                }
            };
        }
        
    }
}