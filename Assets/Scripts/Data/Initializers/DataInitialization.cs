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
            FileUtils.LoadJSON(FileUtils.ResolvePath("_USER_DATA"), out JSONNode userJSON);

            Core.GMApplication.Create(userJSON, gameJSON);

            GameState.Restore(userJSON);

            SceneManager.LoadScene("GameScene");
        }
    }
}
