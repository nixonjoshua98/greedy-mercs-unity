using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs
{
    using GM.Armoury;

    using GreedyMercs.Armoury.Data;

    [System.Serializable]
    public class UpgradeState
    {
        public int level = 1;
    }

    public class GameDataInitializer : MonoBehaviour
    {
        [Header("Scriptables")]
        [SerializeField] ArmourySO Armoury;

        [SerializeField] LootItemListSO RelicList;
        [SerializeField] SkillListSO SkillList;
        [SerializeField] BountyListSO BountyList;
        [SerializeField] CharacterListSO CharacterList;

        void Awake()
        {
            StaticData.AssignScriptables(SkillList, BountyList, CharacterList, RelicList, Armoury);
        }

        void Start()
        {
            Debug.Log(Application.persistentDataPath);

            Server.GetGameData(ServerGameDataCallback);
        }

        void ServerLoginCallback(long code, string body)
        {
            JSONNode node;

            if (code == 200)
                node = Utils.Json.Decompress(body);

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required to play.");

                return;
            }

            InstantiatePlayerData(node);

            SceneManager.LoadScene("GameScene");
        }

        void ServerGameDataCallback(long code, string compressedJson)
        {
            JSONNode node;

            if (code == 200)
                node = Utils.Json.Decompress(compressedJson);

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");

                return;
            }

            InstantiateServerData(node);

            Server.Login(ServerLoginCallback, Utils.Json.GetDeviceNode());
        }

        void InstantiatePlayerData(JSONNode node)
        {
            GameState.Restore(node);

            ArmouryManager.Create(node["armoury"]);
        }

        void InstantiateServerData(JSONNode node)
        {
            StaticData.Restore(node);
        }
    }
}