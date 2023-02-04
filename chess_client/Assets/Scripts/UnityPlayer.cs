using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chess.Model;
using Net;
using UnityEngine;

public class UnityPlayer : MonoBehaviour
{
    private void Start()
    {
        TaskScheduler.UnobservedTaskException +=
            (_, e) => Debug.LogException(e.Exception);
        
        var id = PlayerPrefs.GetString("PlayerId");
        if (string.IsNullOrEmpty(id))
        {
            PlayerPrefs.SetString("PlayerId", Guid.NewGuid().ToString());
        }
    }
    
}
