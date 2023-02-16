using System.Collections;
using chess_shared.Net;
using Chess.Model;
using Net;
using UnityEngine;

public class NetView : MonoBehaviour
{
    [SerializeField] private GameObject startUi;
    [SerializeField] private DeskView deskView;
    private ChessNetClient ChessNetClient;

    public void JoinVsPlayer()
    {
        Join(GameMode.RealEnemy);
    }

    public void JoinVsBot()
    {
        Join(GameMode.ServerBot);
    }

    private async void Join(GameMode gameMode)
    {
        ChessNetClient = deskView.ChessNetClient;
        await ChessNetClient.Join(new JoinArgs()
        {
            GameMode = gameMode,
            PlayerId = PlayerPrefs.GetString("PlayerId")
        });
        GetComponent<UserInput>().UserColor = ChessNetClient.Color;
        if (ChessNetClient.Color == ChessColor.Black)
        {
            deskView.BuildMap();
            deskView.transform.RotateAround(new Vector3(3.5f, 3.5f,0), Vector3.forward, 180);
        }
        StartCoroutine(CheckNews());
        startUi.SetActive(false);
        var joinResult = ChessJsonSerializer.SerializeObj(ChessNetClient.joinResult);
        PlayerPrefs.SetString("JoinResult", joinResult);
    }

    public void ConnectToGame()
    {
        ChessNetClient = deskView.ChessNetClient;
        GetComponent<UserInput>().UserColor = ChessNetClient.Color;
        deskView.BuildMap();
        if (ChessNetClient.Color == ChessColor.Black)
        {
            deskView.transform.RotateAround(new Vector3(3.5f, 3.5f,0), Vector3.forward, 180);
        }
        StartCoroutine(CheckNews());
        startUi.SetActive(false);
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
