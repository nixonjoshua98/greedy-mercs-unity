using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

namespace GreedyMercs.StageDM.Prestige
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
            JSONNode node = Utils.Json.GetDeviceNode();

            node.Add("prestigeStage", GameState.Stage.stage);

            SquadManager.ToggleAttacking(false);

            Server.Prestige(this, (code, compressed) => { OnPrestigeCallback(code, compressed, callback); }, node);
        }

        void OnPrestigeCallback(long code, string compressed, Action<bool> callback)
        {
            if (code == 200)
            {
                DataManager.IsPaused = true;

                Events.OnPlayerPrestige.Invoke();

                GameState.SaveLocalDataOnly();

                Utils.File.WriteJson(DataManager.DATA_FILE, Utils.Json.Decompress(compressed));

                StartCoroutine(RunPrestigeAnimation());
            }

            else
            {
                Utils.UI.ShowMessage("Server Connection", "Failed to contact the server :(");

                SquadManager.ToggleAttacking(true);
            }

            callback(code == 200);
        }

        IEnumerator RunPrestigeAnimation()
        {
            CancelInvoke("UpdatePanel");

            yield return SquadManager.MoveOut(1.0f);

            bool _ = Utils.File.ReadJson(DataManager.DATA_FILE, out JSONNode node);

            GameState.Restore(node);

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}
