using System;
using System.Collections;
using System.Collections.Generic;
using Chess.Model;
using Chess.View;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private DeskView deskView;
    [SerializeField] private ChessColor color;
    private Player player;

    private void Start()
    {
        player = color == ChessColor.White ? deskView.model.WhitePlayer : deskView.model.BlackPlayer;
    }

    private void Display()
    {
        foreach (var pieces in player.capturedPieces)
        {
            if (pieces.Count > 0)
            {
                
            }
        }
    }
}
