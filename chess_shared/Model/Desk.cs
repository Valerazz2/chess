using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Model.Pieces;
using Model;
using Newtonsoft.Json;

namespace Chess.Model
{
    public class Desk
    {
        public static readonly int DeskSizeX = 8, DeskSizeY = 8;
        [JsonIgnore] public IEnumerable<Square> ISquares => Squares.Cast<Square>(); 
        
        public ChessColor Move = ChessColor.White;
        
        private readonly Square[,] Squares = new Square[DeskSizeX, DeskSizeY];
        
        public Piece CurrentPiece { get; set; }

        private ChessState ChessState = ChessState.PieceNull;

        public event Action<MoveInfo> OnMove;
        public event Action<MoveInfo> OnServerMove;
        public event Action<Piece> OnPieceCaptured;
        
        public Player WhitePlayer, BlackPlayer;

        [JsonIgnore] public MoveInfo prevMove = new();

        public ObservableList<Piece> Pieces = new();

        public void Clear()
        {
            Pieces.Clear();
            BlackPlayer.capturedPieces.Clear();
            WhitePlayer.capturedPieces.Clear();
            foreach (var square in Squares)
            {
                square.Piece = null;
            }
        }
        
        public void CreateMap()
        {
            var figuresSpots = new Piece[,]
            {
                {new Rook(this), new Pawn(this), null, null, null, null, new Pawn(this), new Rook(this)},
                {new Knight(this), new Pawn(this), null, null, null, null, new Pawn(this), new Knight(this)},
                {new Bishop(this), new Pawn(this), null, null, null, null, new Pawn(this), new Bishop(this)},
                {new Queen(this), new Pawn(this), null, null, null, null, new Pawn(this), new Queen(this)},
                {new King(this), new Pawn(this), null, null, null, null, new Pawn(this), new King(this)},
                {new Bishop(this), new Pawn(this), null, null, null, null, new Pawn(this), new Bishop(this)},
                {new Knight(this), new Pawn(this), null, null, null, null, new Pawn(this), new Knight(this)},
                {new Rook(this), new Pawn(this), null, null, null, null, new Pawn(this), new Rook(this)}
            };

            for (var x = 0; x < DeskSizeX; x++)
            {
                for (var y = 0; y < DeskSizeY; y++)
                {
                    var color = (x + y) % 2 == 0 ? ChessColor.Black : ChessColor.White;
                    var piece = figuresSpots[x, y];
                    var tile = Squares[x, y] = new Square(new Vector2Int(x, y), color, piece, this);
                    if (piece != null)
                    {
                        piece.Square = tile;
                        piece.Color = y <= 2 ? ChessColor.White : ChessColor.Black;
                        Pieces.Add(piece);
                    }
                }
            }
            WhitePlayer = new Player(ChessColor.White, this);
            BlackPlayer = new Player(ChessColor.Black, this);
        }

        public Piece AddPiece(ChessColor color, PieceType type, Square square)
        {
            var piece = type.GetNewPieceByType(this);
            piece.Color = color;
            piece.Square = square;
            square.Piece = piece;
            Pieces.Add(piece);
            return piece;
        }

        public void MoveTo(Piece piece, Square target)
        {
            ResetTiles(false);
            if (!piece.AbleMoveTo(target) || !piece.TryMoveSuccess(target))
            {
                return;
            }
            
            piece.Square.Marked.Value = true;
            var wantTakeOnThePass = piece.GetPieceType() == PieceType.Pawn &&
                                    Math.Abs(piece.Square.Pos.X - target.Pos.X) == 1 && target.Piece == null;

            Move = Move.Invert();
            
            if (WantCastling(target, piece))
            {
                var rook = FindFirstFigureByStep(target, piece);
                MoveRookWhenCastling(rook, piece);
            }
            
            if (target.Piece != null)
            {
                OnPieceCaptured?.Invoke(target.Piece);
                Pieces.Remove(target.Piece);
            }

            var eventInfo = new MoveInfo
            {
                MoveColor = Move.Invert(),
                Piece = piece,
                MovedFrom = piece.Square,
            };
            
            piece.MoveTo(target);

            if (piece.GetPieceType() == PieceType.Pawn && piece.ReachedLastSquare())
            {
                AddPiece(piece.Color, PieceType.Queen, piece.Square);
                Pieces.Remove(piece);
            }
            
            prevMove.MovedFrom = eventInfo.MovedFrom;
            prevMove.Piece = piece;
            piece.Square.Marked.Value = true;
            
            OnServerMove?.Invoke(eventInfo);
            OnMove?.Invoke(eventInfo);
            
            if (wantTakeOnThePass)
            {
                OnTakeOnThePass(eventInfo.MovedFrom, piece);
            }
        }

        private bool WantCastling(Square target, Piece piece)
        {
            return piece.GetPieceType() == PieceType.King &&
                   Vector2Int.Distance(piece.Square.Pos, target.Pos) == new Vector2Int(2, 0);
        }

        private void OnTakeOnThePass(Square movedFrom, Piece piece)
        {
            var deltaY = movedFrom.Pos.Y - piece.Square.Pos.Y;
            var square = GetSquareAt(piece.Square.Pos + new Vector2Int(0, deltaY));
            OnPieceCaptured?.Invoke(square.Piece);
            Pieces.Remove(square.Piece);
            square.Piece = null;
        }

        public Piece FindKing(ChessColor color)
        {
            return FindPieceColor(color).FirstOrDefault(figure => figure.GetPieceType() == PieceType.King);
        }

        public bool MateFor(Piece king)
        {
            return !FindPieceColor(king.Color).Any(figure => figure.AbleMoveAnyWhere()) && IsCheckTo(king);
        }

        public bool StaleMateFor(ChessColor color)
        {
            return FindPieceColor(color).All(piece => !piece.AbleMoveAnyWhere());
        }

        public bool IsCheckTo(Piece king)
        {
            var oppositeColor = king.Color.Invert();
            return FindPieceColor(oppositeColor).Any(figure => figure.AbleMoveTo(king.Square));
        }

        public IEnumerable<Piece> FindPieceColor(ChessColor chessColor)
        {
            var figures = new List<Piece>();
            foreach (var tile in Squares)
            {
                var piece = tile.Piece;
                if (piece != null && piece.Color == chessColor)
                {
                    figures.Add(piece);
                }
            }
            return figures;
        }
        
        public Piece FindFirstFigureByStep(Square target, Piece king)
        {
            var step = king.Square.Pos.GetStep(target.Pos);
            var pos = king.Square.Pos + step;
            while (pos.X < DeskSizeX && pos.X >= 0)
            {
                var piece = GetPieceAt(pos);
                var enemyColor = king.Color.Invert();
                if (piece != null || FindPieceColor(enemyColor).Any(e => e.AbleMoveTo(Squares[pos.X, pos.Y])))
                {
                    return piece;
                }

                pos += step;
            }
            return null;
        }


        private void MoveRookWhenCastling(Piece rook, Piece king)
        {
            var offset = king.Square.Pos.GetStep(rook.Square.Pos);
            var rookPos = king.Square.Pos + offset;
            var moveInfo = new MoveInfo
            {
                Piece = rook,
                MovedFrom = rook.Square,
            };
            rook.MoveToWithOutChecking(Squares[rookPos.X, rookPos.Y]);
            OnMove?.Invoke(moveInfo);
        }
        
        public Piece GetPieceAt(Vector2Int pos)
        {
            return Squares[pos.X, pos.Y].Piece;
        }

        public Square GetSquareAt(Vector2Int pos) {
            return Squares[pos.X, pos.Y];
        }

        public Square GetSquareAt(string pos)
        {
            return GetSquareAt(new Vector2Int(pos[0] - 'a', pos[1] - '1'));
        }

        public void Select(Square square, ChessColor color)
        {
            if (color != Move)
            {
                return;
            }
            switch (ChessState)
            {
                case ChessState.PieceNull:
                    if (square.IsPieceOfColor(Move))
                    {
                        CurrentPiece = square.Piece;
                        SetMoveAbleSquaresFor(CurrentPiece);
                        ChessState = ChessState.PieceChoosed;
                    }
                    break;
                
                case ChessState.PieceChoosed:
                    if (square.IsPieceOfColor(Move))
                    {
                        CurrentPiece = square.Piece;
                        SetMoveAbleSquaresFor(CurrentPiece);
                    }
                    else if (CurrentPiece.Color == Move)
                    {
                        MoveTo(CurrentPiece, square);
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void Select(string squareRef, ChessColor color)
        {
            var x = squareRef[0] - 'a';
            var y = squareRef[1] - '1';
            if (x < 0 || x > DeskSizeX - 1 || y < 0 || y > DeskSizeY - 1)
            {
                throw new Exception($"There is no square by ({squareRef})");
            }
            Select(GetSquareAt(new Vector2Int(x, y)), color);
        }
        private void SetMoveAbleSquaresFor(Piece piece)
        {
            ResetTiles(false);
            foreach (var square in Squares)
            {
                square.MoveAble.Value = piece.AbleMoveTo(square) && piece.TryMoveSuccess(square);
            }

            piece.Square.Marked.Value = true;
        }
        
        private void ResetTiles(bool moveAble)
        {
            foreach (var square in Squares)
            {
                square.MoveAble.Value= moveAble;
                square.Marked.Value = false;
            }
        }
    }
}
