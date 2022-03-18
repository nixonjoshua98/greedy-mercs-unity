using GM.HTTP;
using GM.HTTP.Requests;
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
                PrestigeStage = App.GameState.Stage
            };

            App.HTTP.Prestige(request, Server_OnPrestige);
        }

        // = Callbacks = //

        void OnPrePrestige()
        {
            App.SaveManager.Paused = true;
            App.SaveManager.DeleteLocalFile();
        }

        void OnPrestigeFailed()
        {
            App.SaveManager.Paused = false;
            App.SaveManager.Save();
        }

        void OnPrestigeSuccess(PrestigeResponse resp)
        {
            App.DeleteLocalStateData();

            App.UpdateDataContainers(resp.UserData, resp.StaticData);

            App.SaveManager.Save();

            SceneManager.LoadScene("GameScene");
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
                OnPrestigeFailed();
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
