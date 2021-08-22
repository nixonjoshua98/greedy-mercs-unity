
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UpgradeState
{
    public int level = 1;
}

namespace GM
{
    using GM.Server;

    public class LoginInitialization : MonoBehaviour
    {
        void Start()
        {
            GetGameData();
        }


        void GetGameData()
        {
            HTTPClient.GetClient().Get("gamedata", (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath(GameData.SERVER_FILE), resp);

                    Login();
                }

                else
                {
                    CanvasUtils.ShowInfo("Server Connection", "Failed to connect to the server");
                }
            });
        }

       

        void Login()
        {
            HTTPClient.GetClient().Post("login", (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath(UserData.SERVER_FILE), resp["userData"]);

                    SceneManager.LoadScene("InitScene", LoadSceneMode.Additive);
                }

                else
                {
                    CanvasUtils.ShowInfo("Server Connection", "Failed to connect to the server");
                }
            });
        }
    }
}