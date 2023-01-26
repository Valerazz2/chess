using System.Collections;
using System.Collections.Generic;
using chess_shared.Net;
using Chess.Model;
using Net;
using UnityEngine;

public class NetView : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private DeskView deskView;
    public ChessNetClient ChessNetClient;

    public async void JoinVsPlayer()
    {
        Join(GameMode.RealEnemy);
    }

    public async void JoinVsBot()
    {
        Join(GameMode.ServerBot);
    }

    private async void Join(GameMode gameMode)
    {
        ChessNetClient = deskView.ChessNetClient;
        await ChessNetClient.Join(new JoinArgs()
        {
            GameMode = gameMode
        });
        GetComponent<UserInput>().UserColor = ChessNetClient.Color;
        if (ChessNetClient.Color == ChessColor.Black)
        {
            deskView.BuildMap();
            deskView.transform.RotateAround(new Vector3(3.5f, 3.5f,0), Vector3.forward, 180);
        }
        StartCoroutine(CheckNews());
        playButton.SetActive(false);
    }
    
    private IEnumerator CheckNews()
    {
        while (true)
        {
            var task = ChessNetClient.CheckNews();
            yield return new WaitUntil(() => task.IsCompleted);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
