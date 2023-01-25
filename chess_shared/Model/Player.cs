using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Chess.Model
{
    public class Player
    {
        public readonly ObservableList<PieceClone> capturedPieces = new();

        private ChessColor color;

        private int GetCapturedValue()
        {
            int value = 0;
            foreach (var pieceTypeClone in capturedPieces.List)
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
            foreach (var pieceClone in capturedPieces.List)
            {
                if (pieceClone.PieceType == piece.GetPieceType())
                {
                    pieceClone.Count++;
                    return;
                }
            }

            var newPieceClone = new PieceClone(color.Invert(), piece.GetPieceType());
            capturedPieces.Add(newPieceClone);
        }
    }
}
