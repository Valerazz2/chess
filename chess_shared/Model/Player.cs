using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Model
{
    public class Player
    {
        public readonly List<PieceClone> capturedPieces = new();
        
        private ChessColor color;

        private int GetCapturedValue()
        {
            int value = 0;
            foreach (var pieceTypeClone in capturedPieces)
            {
                value += pieceTypeClone.Count * pieceTypeClone.PieceType.GetPrice();
            }
            return value;
        }

        public Player(ChessColor chessColor, Desk desk)
        {
            color = chessColor;
            capturedPieces.Add(new PieceClone(color, desk, PieceType.Pawn));
            capturedPieces.Add(new PieceClone(color, desk, PieceType.Knight));
            capturedPieces.Add(new PieceClone(color, desk, PieceType.Bishop));
            capturedPieces.Add(new PieceClone(color, desk, PieceType.Rook));
            capturedPieces.Add(new PieceClone(color, desk, PieceType.Queen));
            capturedPieces.Add(new PieceClone(color, desk, PieceType.King));
        }
    }
}
