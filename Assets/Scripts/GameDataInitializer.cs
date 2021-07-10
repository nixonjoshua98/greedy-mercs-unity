using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UpgradeState
{
    public int level = 1;
}

namespace GM
{
    using GM.Inventory;
    using GM.Artefacts;
    using GM.Characters;

    public class GameDataInitializer : MonoBehaviour
    {
        [SerializeField] SkillListSO SkillList;

        void Awake()
        {
            UserData.CreateInstance();

            StaticData.AssignScriptables(SkillList);
        }

        void Start()
        {
            Server.Get("gamedata", ServerGameDataCallback);
        }

        void ServerLoginCallback(long code, JSONNode resp)
        {
            if (code == 200)
            {
                InstantiatePlayerData(resp);

                SceneManager.LoadScene("GameScene");
            }

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required to play.");
            }
        }

        void ServerGameDataCallback(long code, JSONNode resp)
        {
            if (code == 200)
            {
                InstantiateServerData(resp);

                Server.Post("login", ServerLoginCallback);
            }

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");
            }
        }

        void InstantiatePlayerData(JSONNode node)
        {
            GameState.Restore(node);

            UserData.Get().UpdateWithServerUserData(node);

            MercenaryManager.Create();

            InventoryManager.Create(node["inventory"]);
            ArtefactManager.Create(node["artefacts"]);
            SkillsManager.Create(node["skills"]);
        }

        void InstantiateServerData(JSONNode node)
        {
            StaticData.Restore(node);
        }
    }
}