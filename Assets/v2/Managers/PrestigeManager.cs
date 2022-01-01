using GM.HTTP;
using UnityEngine.SceneManagement;
using UnityEngine;
using GM.HTTP.Requests;

namespace GM.Managers
{
    public class PrestigeManager : Core.GMMonoBehaviour
    {
        public void OnPrestigeButton()
        {
            App.SaveManager.Paused = true;
            App.SaveManager.DeleteLocalFile();

            App.HTTP.Prestige(OnPrestigeResponse);
        }

        void OnPrestigeResponse(PrestigeResponse resp)
        {
            if (resp.StatusCode == HTTPCodes.Success)
            {
                OnPrestigeSuccess(resp);
            }
            else
            {
                App.SaveManager.Paused = false;
                App.SaveManager.Save();

                Debug.Log("Prestige failed");
            }
        }

        void OnPrestigeSuccess(PrestigeResponse resp)
        {
            App.Data.Update(resp.UserData, resp.StaticData);

            App.Data.ResetPrestigeData();

            App.SaveManager.Save();

            SceneManager.LoadSceneAsync("GameScene");
        }
    }
}
