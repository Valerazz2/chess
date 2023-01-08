using Chess.Model;
using Chess.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapturedPieceView : AbstractView<PieceClone>
{
    [SerializeField] private TextMeshProUGUI capturedPieceCountTxt;

    protected override void OnBind()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("pieces/" + model.PieceType + "_" + model.Color);
        model.CountChanged += Display;
    }

    public void CreateViewFor(PieceClone pieceClone)
    {
        Bind(pieceClone);
    }

    private void Display()
    {
        capturedPieceCountTxt.text = model.Count.ToString();
    }
}
