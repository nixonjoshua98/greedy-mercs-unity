using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs
{
    using GM.Bounty;
    using GM.Armoury;
    using GM.Inventory;
    using GM.BountyShop;

    [System.Serializable]
    public class UpgradeState
    {
        public int level = 1;
    }

    public class GameDataInitializer : MonoBehaviour
    {
        [SerializeField] LootItemListSO RelicList;
        [SerializeField] SkillListSO SkillList;
        [SerializeField] CharacterListSO CharacterList;

        void Awake()
        {
            StaticData.AssignScriptables(SkillList, CharacterList, RelicList);
        }

        void Start()
        {
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

            Server.Login(ServerLoginCallback, Utils.Json.GetDeviceInfo());
        }

        void InstantiatePlayerData(JSONNode node)
        {
            GameState.Restore(node);

            ArmouryManager.Create(node["armoury"]);
            BountyManager.Create(node["bounties"]);
            InventoryManager.Create(node["inventory"]);
            BountyShopManager.Create(node["bountyShop"]);
        }

        void InstantiateServerData(JSONNode node)
        {
            StaticData.Restore(node);
        }
    }
}