using System.Text.Json;
using chess_shared.Net;
using Chess.Model;
using Microsoft.AspNetCore.Mvc;
using Net;
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

    public string Join([FromBody] JsonElement args)
    {
        var joinArgs = JsonSerializer.DeserializeObj<JoinArgs>(args.ToString());
        var result = Server.Join(joinArgs);
        var ret = JsonSerializer.SerializeObj(result);
        log.Info("Join:result:" + ret);
        return ret;
    }
    
    public string MovePiece([FromBody] JsonElement args)
    {
        var movePieceArgs = JsonSerializer.DeserializeObj<MovePieceArgs>(args.ToString());
        Server.MovePiece(movePieceArgs);
        return JsonSerializer.SerializeObj(new MoveResult());
    }

    public string AskNews([FromBody] JsonElement args)
    {
        var askNewsArgs = JsonSerializer.DeserializeObj<AskNewsArgs>(args.ToString());
        if (askNewsArgs.NewsID.Count > 0)
        {
            var player = Server.GetPlayer(askNewsArgs.Sid);
            player.DeleteAppliedNews(askNewsArgs.NewsID);
        }
        var askNewsResult = Server.AskNews(new AskNewsArgs
        {
            Sid = askNewsArgs.Sid
        });
        var result = JsonSerializer.SerializeObj(askNewsResult);
        return result;
    }
}