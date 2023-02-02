using System;
using System.Threading.Tasks;
using Chess.Model;
using Chess.View;
using Net;
using UnityEngine;

public class DeskView : AbstractView<Desk>
{
    [SerializeField] private SquareView squareViewPrefab;
    [SerializeField] private PieceView pieceViewPrefab;
    [SerializeField] private NetView netView;
    [SerializeField] private PlayerView player1View;
    [SerializeField] private PlayerView player2View;
    private SquareView choosedSquare;
    public ChessNetClient ChessNetClient;
    private Desk desk;

    private async void Start()
    {
        desk = new Desk();
        desk.CreateMap();

        TaskScheduler.UnobservedTaskException +=
            (_, e) => Debug.LogException(e.Exception);
        var id = PlayerPrefs.GetString("PlayerId", null);
        if (id == null)
        {
            PlayerPrefs.SetString("PlayerId", Guid.NewGuid().ToString());
        }
        ChessNetClient = new ChessNetClient(desk, id);
        if (await PlayerInGame(id))
        {
            Desk desk = ChessNetClient.GetDeskFor(id);
        }
        netView.ChessNetClient = ChessNetClient;
        ChessNetClient.EnemyJoined += BuildMap;
    }

    private async Task<bool> PlayerInGame(string id)
    {
        return await ChessNetClient.InGame(id);
    }
    
    protected override void OnBind()
    {
        CreateViews(model.ISquares, squareViewPrefab, transform);
        if (ChessNetClient.Color == ChessColor.Black)
        {
            SetModelForPlayerViews(player2View, player1View);
            RotatePieces(180);
        }
        else if(ChessNetClient.Color == ChessColor.White)
        {
            SetModelForPlayerViews(player1View, player2View);
            RotatePieces(0);
        }
        CreateViews(model.GetAllPiece(), pieceViewPrefab, transform);
        model.Pieces.ObjectAdded += CreateView2;
    }

    private void SetModelForPlayerViews(PlayerView player1, PlayerView player2)
    {
        player1.SetModel(model.WhitePlayer);
        player2.SetModel(model.BlackPlayer);
    }
    public void BuildMap()
    {
        Bind(desk);
    }

    private void CreateView2(Piece piece)
    {
        CreateView(piece, pieceViewPrefab, transform);
    }

    private void RotatePieces(float angle)
    {
        pieceViewPrefab.transform.rotation = new Quaternion(0,0,angle,0);
    }
}
