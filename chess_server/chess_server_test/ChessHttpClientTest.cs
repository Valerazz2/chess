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
        var whiteJoinResult = await client.Join();
        Assert.True(whiteJoinResult.Color == ChessColor.White);
        var blackJoinResult = await client.Join();
        Assert.True(blackJoinResult.Color == ChessColor.Black);
        await client.OnMove(new MovePieceArgs()
        {
            Sid = whiteJoinResult.Sid,
            MovedFrom = "e2",
            MovedTo = "e4"
        });
        
        
    }
}