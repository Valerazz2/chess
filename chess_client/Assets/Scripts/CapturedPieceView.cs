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
        capturedPieceCountTxt.text = (model.Count.Value + 1).ToString();
        model.Count.ValueChanged += Display;
    }

    private void Display(int arg1, int arg2)
    {
        capturedPieceCountTxt.text = (model.Count.Value + 1).ToString();
    }

    public void CreateViewFor(PieceClone pieceClone)
    {
        Bind(pieceClone);
    }
}
