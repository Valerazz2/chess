using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Model
{
    public class Player
    {
        public readonly List<PieceClone> capturedPieces = new();
        
        private ChessColor color;

        public event Action<int> NewTypePieceCaptured;

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
            desk.OnPieceCaptured += AddCapturedPiece;
        }

        private void AddCapturedPiece(Piece piece)
        {
            if (piece.Color == color)
            {
                return;
            }
            foreach (var pieceClone in capturedPieces)
            {
                if (pieceClone.PieceType == piece.GetPieceType())
                {
                    pieceClone.Count++;
                    return;
                }
            }

            var newPieceClone = new PieceClone(color.Invert(), piece.GetPieceType());
            capturedPieces.Add(newPieceClone);
            NewTypePieceCaptured?.Invoke(capturedPieces.Count - 1);
        }
    }
}
