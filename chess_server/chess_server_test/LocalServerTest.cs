using System;
using System.Collections.Generic;
using System.Threading;
using chess_shared.Net;
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
        /*Server.Clear();
        var joinW = Server.Join(new JoinArgs());
        Assert.AreEqual(ChessColor.White, joinW.Color);
        var joinB = Server.Join(new JoinArgs());
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(Server.GameCount == 1);
        joinW = Server.Join(new JoinArgs());
        Assert.AreEqual(ChessColor.White, joinW.Color);
        Assert.True(Server.GameCount == 1);
        joinB = Server.Join(new JoinArgs());
        Assert.AreEqual(ChessColor.Black, joinB.Color);
        Assert.True(Server.GameCount == 2);
        var playerWhite = Server.GetPlayer(joinW.Sid);
        Assert.True(playerWhite.game.Desk.Move == ChessColor.White);
        var playerBlack = Server.GetPlayer(joinB.Sid);
        Assert.True(playerBlack.NewsForClient.Count == 0);
        Server.MovePiece(new MovePieceArgs
        {
            Sid = joinW.Sid,
            MovedFrom = "e2",
            MovedTo = "e4"
        });
        Assert.True(playerWhite.game.Desk.Move == ChessColor.Black);
        Assert.True(playerBlack.NewsForClient.Count == 1);
        var New=Server.AskNews(new AskNewsArgs
        {
            Sid = joinB.Sid,
        });
        Assert.True(New.News.Count == 1);
        EnemyFigureMoved news = (EnemyFigureMoved)New.News[0];
        Assert.True(news != null);
        Assert.True(news.MovedFrom == "e2");
        Assert.True(news.MovedTo == "e4");
        Assert.True(playerBlack.NewsForClient.Count == 0);
        Assert.True(playerBlack.game.Desk.Move == ChessColor.Black);
        Assert.True(Server.GetPlayer(joinB.Sid) != null);
        Assert.True(Server.GameCount == 2);*/
    }
    
    private void TestJoin()
    {
        for (var i = 0; i < 10; i++)
        {
            Server.Join(new JoinArgs());
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