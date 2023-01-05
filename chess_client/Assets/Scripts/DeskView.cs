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
    [SerializeField] private PlayerView playerWhiteView;
    [SerializeField] private PlayerView playerBlackView;
    private SquareView choosedSquare;
    public ChessNetClient ChessNetClient;
    private Desk desk;

    private void Start()
    {
        TaskScheduler.UnobservedTaskException +=
            (_, e) => Debug.LogException(e.Exception);
        desk = new Desk();
        desk.CreateMap();
        ChessNetClient = new ChessNetClient(desk);
        netView.Desk = desk;
        netView.ChessNetClient = ChessNetClient;
        ChessNetClient.EnemyJoined += BuildMap;
    }
    protected override void OnBind()
    {
        CreateViews(model.ISquares, squareViewPrefab);
        if (ChessNetClient.Color == ChessColor.Black)
        {
            RotatePieces(180);
        }
        else if(ChessNetClient.Color == ChessColor.White)
        {
            RotatePieces(0);
        }
        CreateViews(model.GetAllPiece(), pieceViewPrefab);
        
        model.OnPieceAdd += CreateView2;
        playerWhiteView.Bind(desk);
        playerBlackView.Bind(desk);
    }
    public void BuildMap()
    {
        Bind(desk);
    }

    private void CreateView2(Piece p)
    {
        CreateView(p, pieceViewPrefab);
    }

    private void RotatePieces(float angle)
    {
        pieceViewPrefab.transform.rotation = new Quaternion(0,0,angle,0);
    }
}
