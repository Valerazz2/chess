using System;
using System.Collections.Generic;
using System.Threading;
using Chess.Model;
using Chess.Server;
using Net;
using NUnit.Framework;

namespace chess_server_test;

public class LocalServerTest
{
    private static Server Server => Server.Instance;
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleJoinTest()
    {
        Server.Clear();
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
        var playerWhite = Server.GetPlayer(joinW.Sid);
        Assert.True(playerWhite != null);
        Assert.True(playerWhite.game.Desk.CurrentPiece.GetPieceType() == PieceType.Rook);
        
        Server.SelectSquare(new SelectSquareArgs()
        {
            Sid = joinW.Sid,
            SquareRef = "e2"
        });
        Assert.True(playerWhite.game.Desk.CurrentPiece.GetPieceType() == PieceType.Pawn);
        var playerBlack = Server.GetPlayer(joinB.Sid);
        Assert.True(playerBlack != null);
        Assert.True(playerBlack.NewsForClient.Count == 0);
        Server.SelectSquare(new SelectSquareArgs
        {
            Sid = joinW.Sid,
            SquareRef = "e4"
        });
        Assert.True(playerBlack.NewsForClient.Count == 1);
        Assert.True(Server.GetPlayer(joinB.Sid) != null);
        Assert.True(Server.GameCount == 2);
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
        Server.Clear();
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

        Assert.AreEqual(50, Server.GameCount);
    }
}