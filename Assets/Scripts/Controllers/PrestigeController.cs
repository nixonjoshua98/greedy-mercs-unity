using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

namespace GM.StageDM.Prestige
{    
    public class PrestigeController : MonoBehaviour
    {
        void Awake()
        {
            PrestigeController[] controllers = FindObjectsOfType<PrestigeController>();

            if (controllers.Length > 1)
            {
                Debug.LogError("Attempted to spawn duplicate prestige controller object");

                Destroy(gameObject);
            }
        }

        public void Prestige(Action<bool> callback)
        {
            JSONNode node = Utils.Json.GetDeviceInfo();

            node.Add("prestigeStage", GameState.Stage.stage);

            Server.Prestige(this, (code, compressed) => { OnPrestigeCallback(code, compressed, callback); }, node);
        }

        void OnPrestigeCallback(long code, string compressed, Action<bool> callback)
        {
            if (code == 200)
            {
                GlobalEvents.OnPlayerPrestige.Invoke();

                RunPrestigeAnimation();
            }

            else
            {
                Utils.UI.ShowMessage("Server Connection", "Failed to contact the server :(");
            }

            callback(code == 200);
        }

        void RunPrestigeAnimation()
        {
            CancelInvoke("UpdatePanel");

            GameState.Prestige();

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}
