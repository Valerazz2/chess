using System;

namespace Chess.Model
{
    public class PieceClone
    {
        public PieceType PieceType;
        private int count = 1;
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
        
        public PieceClone(ChessColor color, PieceType pieceType)
        {
            Color = color;
            PieceType = pieceType;
        }
        
    }
}