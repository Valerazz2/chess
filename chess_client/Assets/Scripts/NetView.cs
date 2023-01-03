using System.Collections;
using System.Collections.Generic;
using Chess.Model;
using Net;
using UnityEngine;

public class NetView : MonoBehaviour
{
    [SerializeField] private GameObject playbutton;
    [SerializeField] private DeskView deskView;
    public ChessNetClient ChessNetClient;
    public Desk Desk;
    
    public async void Join()
    {
        ChessNetClient = deskView.ChessNetClient;
        await ChessNetClient.Join();
        if (ChessNetClient.Color == ChessColor.Black)
        {
            gameObject.transform.Rotate(Vector3.forward, 180);
        }
        GetComponent<UserInput>().UserColor = ChessNetClient.Color;
        if (ChessNetClient.Color == ChessColor.Black)
        {
            deskView.BuildMap();
        }
        StartCoroutine(CheckNews());
        playbutton.SetActive(false);
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
