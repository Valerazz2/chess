using System;
using System.Threading.Tasks;
using chess_shared.Net;
using Chess.Model;
using Chess.View;
using Net;
using UnityEngine;

public class DeskView : AbstractView<Desk>
{
    [SerializeField] private SquareView squareViewPrefab;
    [SerializeField] private PieceView pieceViewPrefab;
    [SerializeField] private PlayerView player1View;
    [SerializeField] private PlayerView player2View;
    public ChessNetClient ChessNetClient;
    private Desk desk;

    private void Start()
    {
        desk = new Desk();
        desk.CreateMap();
        UnityPlayer.CheckOrSetGuid();
        var serializedDesk = PlayerPrefs.GetString("Desk");
        if (!string.IsNullOrEmpty(serializedDesk))
        {
            ChessJsonSerializer.Populate(serializedDesk, desk);
        }
        ChessNetClient = new ChessNetClient(desk, PlayerPrefs.GetString("PlayerId"));
        ChessNetClient.EnemyJoined += BuildMap;
        desk.OnServerMove += SerializeDesk;
    }

    private void SerializeDesk(MoveInfo obj)
    {
        var serializedDesk = ChessJsonSerializer.SerializeObj(model);
        PlayerPrefs.SetString("Desk", serializedDesk);
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
        CreateViews(model.Pieces.List, pieceViewPrefab, transform);
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
