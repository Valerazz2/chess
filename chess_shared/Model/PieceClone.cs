using System;
using chess_shared.Model;

namespace Chess.Model
{
    public class PieceClone
    {
        public PieceType PieceType;
        public Holder<int> Count = new();

        public ChessColor Color;

        public PieceClone(ChessColor color, PieceType pieceType)
        {
            Color = color;
            PieceType = pieceType;
        }
        
    }
}