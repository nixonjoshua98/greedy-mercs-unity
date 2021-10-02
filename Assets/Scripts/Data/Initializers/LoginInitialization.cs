
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UpgradeState
{
    public int level = 1;
}

namespace GM
{
    using GM.HTTP;

    public class LoginInitialization : MonoBehaviour
    {
        void Start()
        {          
            GameData();
        }


        void GameData()
        {
            HTTPClient.Instance.Get("gamedata", (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath("_GAME_DATA"), resp);

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
            HTTPClient.Instance.Post("login", (code, resp) => {

                if (code == 200)
                {
                    FileUtils.WriteJSON(FileUtils.ResolvePath("_USER_DATA"), resp);

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