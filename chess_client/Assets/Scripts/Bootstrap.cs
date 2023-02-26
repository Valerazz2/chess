using System;
using System.Collections;
using System.Collections.Generic;
using chess_shared.Net;
using Chess.Model;
using Chess.Server;
using Common.Util.Undo;
using Model;
using Net;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private DeskView deskView;
    [SerializeField] private NetView netView;
    private DeskHistory deskHistory;
    private void Start()
    {
        UnityPlayer.CheckOrSetGuid();
        var desk = new Desk();
        desk.CreateMap();
        desk.OnServerMove += SerializeDesk;
        deskHistory = new DeskHistory(desk);
        var chessNetClient = new ChessNetClient(desk, PlayerPrefs.GetString("PlayerId"));

        deskView.Desk = desk;
        deskView.ChessNetClient = chessNetClient;
        deskView.Activate();
        
        netView.ChessNetClient = chessNetClient;
        var serializedDesk = PlayerPrefs.GetString("Desk");
        if (!string.IsNullOrEmpty(serializedDesk))
        {
            desk.Clear();
            var joinResult = PlayerPrefs.GetString("JoinResult");
            chessNetClient.joinResult = ChessJsonSerializer.Deserialize<JoinResult>(joinResult);
            ChessJsonSerializer.Populate(serializedDesk, new DeskObj(desk));
            netView.ConnectToGame();
        }
    }
    
    public void UndoLastMove()
    {
        if (deskHistory.undoManager.IsUndoAvailable)
        {
            deskHistory.undoManager.Undo();
        }
    }

    public void RedoLastMove()
    {
        if (deskHistory.undoManager.IsRedoAvailable)
        {
            deskHistory.undoManager.Redo();
        }
    }
    
    
    private void SerializeDesk(MoveInfo obj)
    {
        var serializedDesk = ChessJsonSerializer.SerializeObj(new DeskObj(deskView.Desk));
        PlayerPrefs.SetString("Desk", serializedDesk);
    }
}
