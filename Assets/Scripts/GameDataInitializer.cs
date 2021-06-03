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
    using GM.Bounty;
    using GM.Armoury;
    using GM.Inventory;
    using GM.BountyShop;
    using GM.Artefacts;
    using GM.Characters;

    public class GameDataInitializer : MonoBehaviour
    {
        [SerializeField] SkillListSO SkillList;

        void Awake()
        {
            StaticData.AssignScriptables(SkillList);
        }

        void Start()
        {
            Server.GetGameData(ServerGameDataCallback);
        }

        void ServerLoginCallback(long code, string body)
        {
            if (code == 200)
            {
                JSONNode node = Funcs.DecryptServerJSON(body);

                InstantiatePlayerData(node);

                SceneManager.LoadScene("GameScene");
            }

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required to play.");
            }
        }

        void ServerGameDataCallback(long code, string compressedJson)
        {
            if (code == 200)
            {
                JSONNode node = Funcs.DecryptServerJSON(compressedJson);

                InstantiateServerData(node);

                Server.Login(ServerLoginCallback, Utils.Json.GetDeviceInfo());
            }

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");
            }
        }

        void InstantiatePlayerData(JSONNode node)
        {
            GameState.Restore(node);

            MercenaryManager.Create();

            ArmouryManager.Create(node["armoury"]);
            BountyManager.Create(node["bounties"]);
            InventoryManager.Create(node["inventory"]);
            BountyShopManager.Create(node["bountyShop"]);
            ArtefactManager.Create(node["artefacts"]);
            SkillsManager.Create(node["skills"]);
        }

        void InstantiateServerData(JSONNode node)
        {
            StaticData.Restore(node);
        }
    }
}