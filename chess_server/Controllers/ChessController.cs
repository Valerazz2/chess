using System.Text;
using chess_shared.Net;
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
    
    public async Task Move()
    {
        var task = await Request.BodyReader.ReadAsync();
        var requestString = EncodingExtensions.GetString(Encoding.UTF8, task.Buffer);
        var args = JsonConvert.DeserializeObject<MovePieceArgs>(requestString);
        if (args != null) Server.MovePiece(args);
    }

    public string AskNews()
    {
        var task = Request.BodyReader.TryRead(out var readResult);
        var requestString = EncodingExtensions.GetString(Encoding.UTF8, readResult.Buffer);
        Request.BodyReader.Complete();
        return requestString;
        /*
        var args = JsonConvert.DeserializeObject<AskNewsArgs>(requestString);

        var player = Server.GetPlayer(args.Sid);
        
        if (args == null) 
            throw new Exception("NoArgs");
        
        var result = Server.AskNews(args);

        return JsonConvert.SerializeObject(result);
        */
    }
}