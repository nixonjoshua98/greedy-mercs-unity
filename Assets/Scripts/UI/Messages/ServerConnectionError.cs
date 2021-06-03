using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public class ServerConnectionError : Message
    {
        public void OnReconnect()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}