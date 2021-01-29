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

            Server.GetGameData(ServerGameDataCallback);
        }

        void ServerLoginCallback(long code, string compressed)
        {
            bool isLocalSave = Utils.File.Read(DataManager.DATA_FILE, out string localSaveJson);

            JSONNode node = JSON.Parse(isLocalSave ? localSaveJson : "{}");

            GameState.Restore(node);

            if (code == 200)
            {
                GameState.UpdateWithServerData(Utils.Json.Decompress(compressed));

                SceneManager.LoadScene(isLocalSave ? "GameScene" : "IntroScene");
            }

            else if (!isLocalSave)
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required");

                return;
            }

            SceneManager.LoadScene(isLocalSave ? "GameScene" : "IntroScene");
        }

        void ServerGameDataCallback(long code, string compressedJson)
        {

            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressedJson);

                StaticData.Restore(node);

                Utils.File.SecureWrite(DataManager.STATIC_FILE, node.ToString());
            }

            else if (Utils.File.SecureRead(DataManager.STATIC_FILE, out string localStaticData))
            {
                StaticData.Restore(JSON.Parse(localStaticData));
            }

            else
            {
                Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");

                return;
            }

            Server.Login(ServerLoginCallback, Utils.Json.GetDeviceNode());
        }
    }
}