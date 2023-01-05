using System.Collections;
using System.Collections.Generic;
using Chess.Model;
using UnityEngine;
using UnityEngine.UI;

public class CapturedPieceView : MonoBehaviour
{
    [SerializeField] private Text capturedPieceCountTxt;
    [SerializeField] private ChessColor color;
    private PieceClone pieceClone;
    private Sprite sprite;

    public void Bind(PieceClone pieceClone)
    {
        this.pieceClone = pieceClone;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("pieces/" + pieceClone.PieceType + "_" + pieceClone.Color);
        this.pieceClone.CountChanged += Display;
    }

    private void Display()
    {
        capturedPieceCountTxt.text = pieceClone.Count.ToString();
    }
}
