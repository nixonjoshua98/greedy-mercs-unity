using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerNotice : Notice
{
    public void TryReconnect()
    {
        SceneManager.LoadSceneAsync(0);
    }
}