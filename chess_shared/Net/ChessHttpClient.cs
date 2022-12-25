using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chess.Server;
using Newtonsoft.Json;

namespace chess_shared.Net;

public class ChessHttpClient
{
    public string EndPoint = "https://localhost:7097/Chess/";
    
    HttpClient _client = new();
    
    public async Task<JoinResult> Join()
    {
        var url = EndPoint + "Join";
        var data = new StringContent("", Encoding.UTF8, "text/json");
        var response = await _client.PostAsync(url, data);
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<JoinResult>(responseJson);
        return result;
    }

    public void SelectSquare(SelectSquareArgs args)
    {
        throw new System.NotImplementedException();
    }
}