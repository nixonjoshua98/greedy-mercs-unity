using GM.HTTP;
using GM.HTTP.Requests;
using UnityEngine.SceneManagement;

namespace GM.Managers
{
    public class PrestigeManager : Core.GMMonoBehaviour
    {
        private static PrestigeManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void PerformPrestige()
        {
            PrestigeRequest request = new()
            {
                PrestigeStage = App.GameState.Stage
            };

            App.HTTP.Prestige(request, Server_OnPrestige);
        }

        // = Callbacks = //

        private void OnPrePrestige()
        {
            App.SaveManager.Paused = true;
            LocalStateFile.DeleteFile();
        }

        private void OnPrestigeFailed()
        {
            App.SaveManager.Paused = false;
            App.SaveLocalStateFile();
        }

        private void OnPrestigeSuccess(PrestigeResponse resp)
        {
            App.Stats.LocalDailyStats.TotalPrestiges++;

            App.DeleteLocalStateData();

            App.UpdateDataContainers(resp.UserData, resp.DataFiles);

            App.SaveLocalStateFile();

            SceneManager.LoadScene("GameScene");
        }

        // = Server Callbacks = //

        private void Server_OnPrestige(PrestigeResponse resp)
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
