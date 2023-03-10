using System;
using Chess.Model.Pieces;

namespace Chess.Model
{
    public enum PieceType
    {
        Pawn, 
        Knight, 
        Bishop,
        Rook, 
        Queen, 
        King
    }

    public static class PieceTypeEx
    {
        private static readonly int[] PRICES = {1, 3, 3, 5, 8, 100000};
        private static readonly char[] FirstChar = {'p', 'k', 'b', 'r', 'q', 'g'};

        public static int GetPrice(this PieceType pieceType)
        {
            return PRICES[(int)pieceType];
        }

        public static PieceType FromChar(char type)
        {
            var index = Array.IndexOf(FirstChar, type);
            if (index == -1) throw new Exception("There is no pieceType by char" + type);
            return (PieceType)Enum.ToObject(typeof(PieceType), index);
        }
        
        public static char ToChar(this PieceType pieceType)
        {
            return FirstChar[(int) pieceType];
        }
        public static Piece GetNewPieceByType(this PieceType type, Desk desk)
        {
            Piece piece = type switch
            {
                PieceType.Pawn => new Pawn(desk),
                PieceType.Knight => new Knight(desk),
                PieceType.Bishop => new Bishop(desk),
                PieceType.Rook => new Rook(desk),
                PieceType.Queen => new Queen(desk),
                PieceType.King => new King(desk),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            return piece;
        }
    }
}