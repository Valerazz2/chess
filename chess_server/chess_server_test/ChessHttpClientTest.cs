using System.Threading.Tasks;
using chess_shared.Net;
using Chess.Model;
using NUnit.Framework;

namespace chess_server_test;

public class ChessHttpClientTest
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task SimpleJoinTest()
    {
        var client = new ChessHttpClient();
        var whiteJoinResult = await client.Join(new JoinArgs{GameMode = GameMode.RealEnemy});
        Assert.True(whiteJoinResult.Color == ChessColor.White);
        var blackJoinResult = await client.Join(new JoinArgs{GameMode = GameMode.RealEnemy});
        Assert.True(blackJoinResult.Color == ChessColor.Black);
    }
}