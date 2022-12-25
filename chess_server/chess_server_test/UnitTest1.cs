using System;
using System.Collections.Generic;
using System.Threading;
using Chess.Model;
using Chess.Server;
using NUnit.Framework;

namespace chess_server_test;

public class Tests
{
    private static Server Server => Server.Instance;
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleJoinTest()
    {
        var joinW = Server.Join();
        Assert.AreEqual(ChessColor.White, joinW.Color);
        var joinB = Server.Join();
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(Server.GameCount == 1);
        joinW = Server.Join();
        Assert.AreEqual(ChessColor.White, joinW.Color);
        Assert.True(Server.GameCount == 1);
        joinB = Server.Join();
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(Server.GameCount == 2);
        var squareParams = new SelectSquareArgs
        {
            Sid = joinW.Sid,
            SquareRef = "a1"
        };
        Server.SelectSquare(squareParams);
        var player = Server.GetPlayer(joinW.Sid);
        Assert.True(player.game.Desk.CurrentPiece.GetPieceType() == PieceType.Rook);
    }
    
    private void TestJoin()
    {
        for (var i = 0; i < 10; i++)
        {
            Server.Join();
        }
    }
    
    [Test]
    public void MultiThreadTest()
    {
        var t = DateTime.Now;
        var threads = new List<Thread>();
        for (var i = 0; i < 10; i++)
        {
            var thread = new Thread(TestJoin);
            thread.Start();
            thread.Name = "" + i;
            threads.Add(thread);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.Out.WriteLine(DateTime.Now - t + ", gameCount=" + Server.GameCount);
    }
}