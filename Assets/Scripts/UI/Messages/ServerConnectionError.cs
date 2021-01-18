using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs
{
    public class ServerConnectionError : Message
    {
        public void OnReconnect()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}