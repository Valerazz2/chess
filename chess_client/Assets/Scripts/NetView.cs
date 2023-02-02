using System.Collections;
using chess_shared.Net;
using Chess.Model;
using Net;
using UnityEngine;

public class NetView : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject playButton2;
    [SerializeField] private DeskView deskView;
    public ChessNetClient ChessNetClient;

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
        playButton.SetActive(false);
        playButton2.SetActive(false);
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
