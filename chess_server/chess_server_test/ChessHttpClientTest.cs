using System.Threading.Tasks;
using chess_shared.Net;
using Chess.Model;
using NUnit.Framework;

namespace chess_server_test;

public class ChessHttpClientTest
{
    private static Server Server => Server.Instance;
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task SimpleJoinTest()
    {
        Server.Clear();
        var client = new ChessHttpClient();
        var whiteJoinResult = await client.Join();
        Assert.NotNull(whiteJoinResult.Sid);
    }
}