using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chess.Model;
using Chess.Server;
using Net;
using Newtonsoft.Json;

namespace chess_shared.Net
{

    public class ChessHttpClient
    {
        public string EndPoint = "https://localhost:7097/Chess/";

        private HttpClient _client = new();

        private async Task<TResult> Query<TResult, TArgs>(string method, TArgs args) where TArgs : class
        {
            var url = EndPoint + method;
            var argsJson = args == null ? "" : ChessJsonSerializer.SerializeObj(args);
            var data = new StringContent(argsJson, Encoding.UTF8, "text/json");
            HttpResponseMessage response = await _client.PostAsync(url, data);
            var responseJson = await response.Content.ReadAsStringAsync();
            response.Dispose();
            var result = ChessJsonSerializer.DeserializeObj<TResult>(responseJson);
            return result;
        }

        public async Task<JoinResult> Join(JoinArgs args)
        {
            return await Query<JoinResult, object>(nameof(Join), args);
        }

        public async Task<MoveResult> MovePiece(MovePieceArgs args)
        {
            return await Query<MoveResult, object>(nameof(MovePiece), args);
        }

        public async Task<AskNewsResult> AskNews(AskNewsArgs args)
        {
            return await Query<AskNewsResult, object>(nameof(AskNews), args);
        }

        public async Task<Desk> GetDeskFor(string id)
        {
            return await Query<Desk, object>(nameof(GetDeskFor), id);
        }
    }
}