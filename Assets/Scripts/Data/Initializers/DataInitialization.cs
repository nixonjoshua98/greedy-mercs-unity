using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public class DataInitialization : MonoBehaviour
    {
        void Awake()
        {
            FileUtils.LoadJSON(FileUtils.ResolvePath("_GAME_DATA"), out JSONNode gameJSON);
            FileUtils.LoadJSON(FileUtils.ResolvePath(UserData.SERVER_FILE), out JSONNode userJSON);

            Core.GMApplication.Create(userJSON, gameJSON);

            GameState.Restore(userJSON);

            UserData.CreateInstance().UpdateWithServerUserData(userJSON);

            SceneManager.LoadScene("GameScene");
        }
    }
}
