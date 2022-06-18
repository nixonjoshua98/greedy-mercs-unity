using GM.HTTP;
using GM.HTTP.Requests;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using GM.UI;

namespace GM.Managers
{
    public class PrestigeManager : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PrestigeConfirmPopup;

        // = Callbacks = //

        private void OnPrePrestige()
        {
            App.SaveManager.Paused = true;
            LocalStateFile.DeleteFile();
        }

        private void OnPrestigeFailed()
        {
            App.SaveManager.Paused = false;
            App.LocalStateFile.WriteToFile();
        }

        private void OnPrestigeSuccess(PrestigeResponse resp)
        {
            App.Stats.LocalDailyStats.TotalPrestiges++;

            App.DeleteLocalStateData();

            App.LocalStateFile.Clear(overwrite: true);

            App.UpdateDataContainers(resp.UserData, resp.DataFiles);

            SceneManager.LoadScene("GameScene");
        }


        private void OnPrestigeResponse(PrestigeResponse resp)
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

        /* Event Listeners */

        private void PerformPrestige()
        {
            App.HTTP.Prestige(OnPrestigeResponse);
        }

        public void OnPrestigeButton()
        {
            var popup = this.InstantiateUI<PrestigePopup>(PrestigeConfirmPopup);

            popup.Initialize(() =>
            {
                OnPrePrestige();
                PerformPrestige();
            });
        }
    }
}
