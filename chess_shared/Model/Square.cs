using chess_shared.Model;
using Newtonsoft.Json;
using Net.Json;

namespace Chess.Model
{
    public class Square : DeskObj
    {
        public readonly Vector2Int Pos;
        public readonly ChessColor Color;
        [JsonIgnore] public Piece Piece;
        private bool _moveAble;
        private bool _marked;
        public Holder<bool> Marked = new();
        public Holder<bool> MoveAble = new();

        public Square(Vector2Int pos, ChessColor color, Piece piece, Desk desk) : base(desk)
        {
            Pos = pos;
            Color = color;
            Piece = piece;
        }

        public void Select(ChessColor color)
        {
            Desk.Select(this, color);
        }

        public bool IsPieceOfColor(ChessColor color)
        {
            return Piece != null && Piece.Color == color;
        }

        public string GetRef()
        {
            var char1 = (char) ('a' + Pos.X);
            var char2 = (char) (Pos.Y + '1');
            return char1.ToString() + char2;
        }
    }
}
