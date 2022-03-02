using GM.HTTP;
using GM.HTTP.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM.Managers
{
    public class PrestigeManager : Core.GMMonoBehaviour
    {
        static PrestigeManager Instance = null;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void PerformPrestige()
        {
            var request = new PrestigeRequest()
            {
                PrestigeStage = GMData.GameState.Stage
            };

            App.HTTP.Prestige(request, Server_OnPrestige);
        }

        // = Callbacks = //

        void OnPrePrestige()
        {
            App.SaveManager.Paused = true;
            App.SaveManager.DeleteLocalFile();
        }

        void OnPrestigeFailed(PrestigeResponse resp)
        {
            App.SaveManager.Paused = false;
            App.SaveManager.Save();

            Debug.Log("Prestige failed");
        }

        void OnPrestigeSuccess(PrestigeResponse resp)
        {
            GMData.DeleteSoftUserData();

            GMData.Update(resp.UserData, resp.StaticData);

            App.SaveManager.Save();

            SceneManager.LoadSceneAsync("GameScene");
        }

        // = Server Callbacks = //

        void Server_OnPrestige(PrestigeResponse resp)
        {
            if (resp.StatusCode == HTTPCodes.Success)
            {
                OnPrestigeSuccess(resp);
            }
            else
            {
                OnPrestigeFailed(resp);
            }
        }

        // = UI Callbacks = //

        public void Button_OnPrestige()
        {
            OnPrePrestige();
            PerformPrestige();
        }
    }
}
