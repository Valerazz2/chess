using System.Text;
using Chess.Model;
using Chess.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    
    public async Task SelectSquare()
    {
        var task = await Request.BodyReader.ReadAsync();
        var requestString = EncodingExtensions.GetString(Encoding.UTF8, task.Buffer);
        var args = JsonConvert.DeserializeObject<SelectSquareArgs>(requestString);
        if (args != null) Server.SelectSquare(args);
    }

    public async Task<string> AskNews()
    {
        var task = await Request.BodyReader.ReadAsync();
        var requestString = EncodingExtensions.GetString(Encoding.UTF8, task.Buffer);
        var args = JsonConvert.DeserializeObject<AskNewsArgs>(requestString);
        
        if (args == null) 
            throw new Exception("NoArgs");
        
        var result = Server.AskNews(args);

        return JsonConvert.SerializeObject(result);
    }
}