using System.Collections.Generic;
using Chess.Model;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private ChessColor color;
    private Player player;
    [SerializeField] private List<CapturedPieceView> capturedPieceViews = new();
    private bool a = true;
    private Desk desk;

    public void Bind(Desk desk)
    {
        this.desk = desk;
        player = color == ChessColor.White ? desk.WhitePlayer : desk.BlackPlayer;
        player.NewTypePieceCaptured += CreateNewView;
    }

    private void CreateNewView(int index)
    {
        var player = color == ChessColor.White ? desk.WhitePlayer : desk.BlackPlayer;
        capturedPieceViews[index].Bind(player.capturedPieces[index]);
    }
}
