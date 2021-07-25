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
    using GM.Data;

    using GM.Artefacts;
    using GM.Units;

    public class GameDataInitializer : MonoBehaviour
    {
        [SerializeField] SkillListSO SkillList;

        void Awake()
        {
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
                CanvasUtils.ShowInfo("Server Connection", "Failed to connect to the server");
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
                CanvasUtils.ShowInfo("Server Connection", "Failed to connect to the server");
            }
        }

        void InstantiatePlayerData(JSONNode node)
        {
            GameState.Restore(node);

            UserData.CreateInstance().UpdateWithServerUserData(node);

            MercenaryManager.Create();

            ArtefactManager.Create(node["artefacts"]);
            SkillsManager.Create(node["skills"]);
        }

        void InstantiateServerData(JSONNode node)
        {
            StaticData.Restore(node);

            GameData.CreateInstance(node);
        }
    }
}