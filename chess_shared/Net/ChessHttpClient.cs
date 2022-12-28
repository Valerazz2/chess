﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chess.Server;
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
            var argsJson = args == null ? "" : JsonConvert.SerializeObject(args);
            var data = new StringContent(argsJson, Encoding.UTF8, "text/json");
            HttpResponseMessage response = await _client.PostAsync(url, data);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(responseJson);
        }

        public async Task<JoinResult> Join()
        {
            return await Query<JoinResult, object>(nameof(IChessService.Join), null);
        }

        public async Task SelectSquare(SelectSquareArgs args)
        {
            await Query<object, SelectSquareArgs>(nameof(IChessService.SelectSquare), args);
        }

        public async Task<AskNewsResult> AskNews(AskNewsArgs args)
        {
            return await Query<AskNewsResult, AskNewsArgs>(nameof(IChessService.AskNews), args);
        }
    }
}