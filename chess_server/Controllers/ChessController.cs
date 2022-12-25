using System.Text;
using Chess.Model;
using Chess.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace chess_server.Controllers;

public class ChessController : Controller
{
    private readonly ILogger<ChessController> _logger;

    private Server server => Server.Instance;
        

    public ChessController(ILogger<ChessController> logger)
    {
        _logger = logger;
    }

    public string Join()
    {
        var result = server.Join();
        return JsonConvert.SerializeObject(result);
    }
    
    public async Task SelectSquare()
    {
        var task = await Request.BodyReader.ReadAsync();
        var requestString = EncodingExtensions.GetString(Encoding.UTF8, task.Buffer);
        var args = JsonConvert.DeserializeObject<SelectSquareArgs>(requestString);
        server.SelectSquare(args);
    }
}