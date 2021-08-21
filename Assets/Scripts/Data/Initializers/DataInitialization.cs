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

            SceneManager.LoadScene("GameScene");
        }


        void RestoreGameData()
        {
            FileUtils.OpenJSON(FileUtils.ResolvePath(GameData.SERVER_FILE), out JSONNode node);

            GameData.CreateInstance(node);
        }


        void RestoreUserData()
        {
            FileUtils.OpenJSON(FileUtils.ResolvePath(UserData.SERVER_FILE), out JSONNode node);

            GameState.Restore(node);

            UserData.CreateInstance().UpdateWithServerUserData(node);

            MercenaryManager.Create();
        }
    }
}
