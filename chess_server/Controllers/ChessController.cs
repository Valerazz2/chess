using System.Text;
using System.Text.Json;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;
using Microsoft.AspNetCore.Mvc;
using Net;
using Newtonsoft.Json;
using JsonSerializer = chess_shared.Net.JsonSerializer;

namespace chess_server.Controllers;

public class ChessController : Controller
{
    private readonly ILogger<ChessController> _logger;

    private static Server Server => Server.Instance;
        

    public ChessController(ILogger<ChessController> logger)
    {
        _logger = logger;
    }

    public string Join()
    {
        var result = Server.Join();
        return JsonConvert.SerializeObject(result);
    }
    
    public void Move([FromBody] string args)
    {
        var movePieceArgs = JsonSerializer.DeserializeObj<MovePieceArgs>(args);
        Server.MovePiece(movePieceArgs);
    }
    
    public string AskNews([FromBody] string args)
    {
        var askNewsArgs = JsonSerializer.DeserializeObj<AskNewsArgs>(args);
        var askNewsResult = Server.AskNews(new AskNewsArgs
        {
            Sid = askNewsArgs?.Sid
        });
        var result = JsonSerializer.SerializeObj(askNewsResult);
        return result;
    }
}