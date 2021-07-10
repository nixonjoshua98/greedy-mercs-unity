using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

namespace GM.StageDM.Prestige
{
    using GM.Events;

    public class PrestigeController : MonoBehaviour
    {
        public void Prestige(Action<bool> callback)
        {
            CurrentStageState state = GameManager.Instance.State();

            JSONNode node = new JSONObject();

            node.Add("prestigeStage", state.currentStage);

            Server.Post("prestige", node, (code, resp) => { OnPrestigeCallback(code, resp, callback); });
        }

        void OnPrestigeCallback(long code, JSONNode resp, Action<bool> callback)
        {
            if (code == 200)
            {
                UserData.Get().UpdateWithServerUserData(resp["completeUserData"]);

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
