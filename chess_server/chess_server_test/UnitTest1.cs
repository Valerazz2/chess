using Chess.Model;
using Chess.Server;
using NUnit.Framework;

namespace chess_server_test;

public class Tests
{
    Server server => Server.Instance;
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleJoinTest()
    {
        var joinW = server.Join();
        Assert.AreEqual(ChessColor.White, joinW.Color);
        var joinB = server.Join();
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(server.GameCount == 1);
        joinW = server.Join();
        Assert.AreEqual(ChessColor.White, joinW.Color);
        Assert.True(server.GameCount == 1);
        joinB = server.Join();
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(server.GameCount == 2);
        var squareParams = new SelectSquareArgs
        {
            Sid = joinW.Sid,
            SquareRef = "00"
        };
        server.SelectSquare(squareParams);
        server.dictionary.TryGetValue(joinW.Sid, out ServerPlayer player);
        Assert.True(player.game.Desk.CurrentPiece.GetPieceType() == PieceType.Rook);
    }
}