using Chess.Server;

namespace Chess.Model;

public class ChessServerBot
{
    private readonly ChessColor color;

    private readonly ChessGame game;
    
    public ChessServerBot(ChessGame game, ChessColor color)
    {
        this.game = game;
        this.color = color;
        this.game.Desk.OnServerMove += AnswerWithMove;
    }

    private void AnswerWithMove(MoveInfo moveInfo)
    {
        if (game.Desk.MateFor(game.Desk.FindKing(color)) || game.Desk.StaleMateFor(color))
        {
            throw new Exception("Mate or staleMate");
        }
        if (moveInfo.Piece.Color == color)
        {
            return;
        }
        var pieces = game.Desk.FindPieceColor(color).ToList();
        while (true)
        {
            Random random = new();
            var piece = pieces[random.Next(pieces.Count - 1)];
            if (piece.AbleMoveAnyWhere())
            {
                List<Square> ableMoveSquares = game.Desk.ISquares.Where(square => piece.AbleMoveTo(square) && piece.TryMoveSuccess(square)).ToList();
                var randomSquare = ableMoveSquares[random.Next(ableMoveSquares.Count - 1)];
                game.Desk.Select(piece.Square, color);
                game.Desk.Select(randomSquare, color);
                return;
            }
        }
    }
}