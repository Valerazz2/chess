using System.Collections;
using System.Threading.Tasks;
using Chess.Model;
using Chess.View;
using Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeskView : AbstractView<Desk>
{
    [SerializeField] private SquareView squareViewPrefab;
    [SerializeField] private PieceView pieceViewPrefab;
    private SquareView choosedSquare;
    private ChessNetClient _chessNetClient;

    private async Task Start()
    {
        TaskScheduler.UnobservedTaskException +=
            (_, e) => Debug.LogException(e.Exception);
        var desk = new Desk();
        _chessNetClient = new ChessNetClient(desk);
        desk.CreateMap();
        Bind(desk);
        await _chessNetClient.Join();
        if (_chessNetClient.Color == ChessColor.Black)
        {
            gameObject.transform.Rotate(Vector3.forward, 180);
        }
        GetComponent<UserInput>().UserColor = _chessNetClient.Color;
        StartCoroutine(CheckNews());
    }

    protected override void OnBind()
    {
        CreateViews(model.ISquares, squareViewPrefab);
        CreateViews(model.GetAllPiece(), pieceViewPrefab);
        
        model.OnPieceAdd += CreateView2;
    }

    private void CreateView2(Piece p)
    {
        CreateView(p, pieceViewPrefab);
    }

    private string GetSquareRef(int x, int y)
    {
        return "" + (char) ('a' + x) + (char) ('1' + y);
    }
    
    private IEnumerator ReloadSceneIn(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator CheckNews()
    {
        while (true)
        {
            var task = _chessNetClient.CheckNews();
            yield return new WaitUntil(() => task.IsCompleted);
            Debug.Log("NewsAsked");
            yield return new WaitForSeconds(1);
        }
    }
}
