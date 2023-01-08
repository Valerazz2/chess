using System.Collections.Generic;
using Chess.Model;
using Chess.View;
using UnityEngine;

public class PlayerView : AbstractView<Player>
{
    [SerializeField] private GameObject spotPref;

    private void CreateNewPieceView(PieceClone pieceClone)
    {
        var spot = Instantiate(spotPref, transform);
        spot.GetComponent<CapturedPieceView>().CreateViewFor(pieceClone);
    }

    public void SetModel(Player player)
    {
        Bind(player);
    }

    protected override void OnBind()
    {
        model.NewPieceTypeCaptured += CreateNewPieceView;
    }
}
