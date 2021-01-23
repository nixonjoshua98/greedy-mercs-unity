using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs
{
    using GreedyMercs.BountyShop.Data;
    using GreedyMercs.Armoury.Data;

    [System.Serializable]
    public class UpgradeState
    {
        public int level = 1;
    }

    public class Initializer : MonoBehaviour
    {
        [Header("Scriptables")]
        [SerializeField] ArmourySO Armoury;

        [SerializeField] BountyShopListSO BountyShop;

        [SerializeField] LootItemListSO RelicList;
        [SerializeField] SkillListSO SkillList;
        [SerializeField] BountyListSO BountyList;
        [SerializeField] CharacterListSO CharacterList;

        void Awake()
        {
            StaticData.AssignScriptables(SkillList, BountyList, CharacterList, RelicList, BountyShop, Armoury);
        }

        void Start()
        {
            Debug.Log(Application.persistentDataPath);

            Server.GetStaticData(this, ServerStaticDataCallback);
        }

        void ServerLoginCallback(long code, string compressed)
        {
            bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

            GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

            if (code == 200)
            {
                // Update the local data with the data from the server
                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.Loot.Update(node["loot"]);
                GameState.Player.Update(node["player"]);
                GameState.Armoury.Update(node["weapons"]);
                GameState.Bounties.Update(node["bounties"]);
                GameState.BountyShop.Update(node["userBountyShop"]);

                SceneManager.LoadScene(isLocalSave ? "GameScene" : "IntroScene");
            }

            else if (!isLocalSave)
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required");

                return;
            }

            SceneManager.LoadScene(isLocalSave ? "GameScene" : "IntroScene");
        }

        void ServerStaticDataCallback(long code, string compressedJson)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressedJson);

                StaticData.Restore(node);

                Utils.File.WriteJson(DataManager.LOCAL_STATIC_FILENAME, node);
            }

            else
            {
                if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localSaveJson))
                {
                    StaticData.Restore(JSON.Parse(localSaveJson));
                }

                else
                {
                    Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");

                    return;
                }
            }

            Server.Login(this, ServerLoginCallback, Utils.Json.GetDeviceNode());
        }
    }
}