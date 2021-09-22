using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    using GM.Data;

    public class DataInitialization : MonoBehaviour
    {
        void Awake()
        {
            RestoreGameData();

            RestoreUserData();

            FileUtils.LoadJSON(FileUtils.ResolvePath(GameData.SERVER_FILE), out JSONNode gameJSON);
            FileUtils.LoadJSON(FileUtils.ResolvePath(UserData.SERVER_FILE), out JSONNode userJSON);

            Core.GMApplication.Create(userJSON, gameJSON);

            SceneManager.LoadScene("GameScene");
        }


        void RestoreGameData()
        {
            FileUtils.LoadJSON(FileUtils.ResolvePath(GameData.SERVER_FILE), out JSONNode node);

            GameData.CreateInstance(node);
        }


        void RestoreUserData()
        {
            FileUtils.LoadJSON(FileUtils.ResolvePath(UserData.SERVER_FILE), out JSONNode node);

            GameState.Restore(node);

            UserData.CreateInstance().UpdateWithServerUserData(node);

            MercenaryManager.Create();
        }
    }
}
