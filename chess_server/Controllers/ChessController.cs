using System.Text.Json;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = chess_shared.Net.JsonSerializer;

namespace chess_server.Controllers;

public class ChessController : Controller
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ChessController));

    private static Server Server => Server.Instance;

    public ChessController()
    {
    }

    public string Join()
    {
        var result = Server.Join();
        var ret = JsonConvert.SerializeObject(result);
        log.Info("Join:result:" + ret);
        return ret;
    }
    
    public string Move([FromBody] JsonElement args)
    {
        var movePieceArgs = JsonSerializer.DeserializeObj<MovePieceArgs>(args.ToString());
        Server.MovePiece(movePieceArgs);
        return JsonConvert.SerializeObject(true);
    }

    public string AskNews([FromBody] JsonElement args)
    {
        var askNewsArgs = JsonSerializer.DeserializeObj<AskNewsArgs>(args.ToString());
        var askNewsResult = Server.AskNews(new AskNewsArgs
        {
            Sid = askNewsArgs?.Sid
        });
        var result = JsonSerializer.SerializeObj(askNewsResult);
        return result;
    }

    public string DeleteAppliedNew([FromBody] JsonElement args)
    {
        var applyNews = JsonSerializer.DeserializeObj<ApplyNews>(args.ToString());
        var player = Server.GetPlayer(applyNews);
        player.DeleteAppliedNews(applyNews);
        return JsonConvert.SerializeObject(true);
    }
    
}