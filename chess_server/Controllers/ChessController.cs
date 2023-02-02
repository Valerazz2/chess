using System.Text.Json;
using chess_shared.Net;
using Chess.Model;
using Microsoft.AspNetCore.Mvc;
using Net;
using Newtonsoft.Json;

namespace chess_server.Controllers;

public class ChessController : Controller
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ChessController));

    private static Server Server => Server.Instance;

    public ChessController()
    {
    }

    public string InGame([FromBody] JsonElement args)
    {
        var id = ChessJsonSerializer.DeserializeObj<string>(args.ToString());
        var result = Server.FindPlayer(id);
        return ChessJsonSerializer.SerializeObj(result);
    }
    public string Join([FromBody] JsonElement args)
    {
        var joinArgs = ChessJsonSerializer.DeserializeObj<JoinArgs>(args.ToString());
        var result = Server.Join(joinArgs);
        var ret = ChessJsonSerializer.SerializeObj(result);
        log.Info("Join:result:" + ret);
        return ret;
    }
    
    public string MovePiece([FromBody] JsonElement args)
    {
        var movePieceArgs = ChessJsonSerializer.DeserializeObj<MovePieceArgs>(args.ToString());
        Server.MovePiece(movePieceArgs);
        return ChessJsonSerializer.SerializeObj(new MoveResult());
    }

    public string AskNews([FromBody] JsonElement args)
    {
        var askNewsArgs = ChessJsonSerializer.DeserializeObj<AskNewsArgs>(args.ToString());
        if (askNewsArgs.NewsID.Count > 0)
        {
            var player = Server.GetPlayer(askNewsArgs.Sid);
            player.DeleteAppliedNews(askNewsArgs.NewsID);
        }
        var askNewsResult = Server.AskNews(new AskNewsArgs
        {
            Sid = askNewsArgs.Sid
        });
        var result = ChessJsonSerializer.SerializeObj(askNewsResult);
        return result;
    }
}