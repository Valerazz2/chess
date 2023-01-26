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
                game.Desk.MoveTo(piece, randomSquare);
                return;
            }
        }
    }
}