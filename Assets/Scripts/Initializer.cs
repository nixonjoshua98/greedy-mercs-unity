using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreedyMercs
{
    [System.Serializable]
    public class UpgradeState
    {
        public int level = 1;
    }

    public class Initializer : MonoBehaviour
    {
        [Header("Scriptables")]
        [SerializeField] LootItemListSO RelicList;
        [SerializeField] SkillListSO SkillList;
        [SerializeField] BountyListSO BountyList;
        [SerializeField] CharacterListSO CharacterList;

        void Awake()
        {
            StaticData.AssignScriptables(SkillList, BountyList, CharacterList, RelicList);
        }

        void Start()
        {
            Debug.Log(Application.persistentDataPath);

            Server.Login(this, ServerLoginCallback, Utils.Json.GetDeviceNode());
        }

        void ServerLoginCallback(long code, string compressed)
        {
            bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

            GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                GameState.Update(node);
            }

            else
            {
                if (!isLocalSave)
                {
                    Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required");

                    return;
                }
            }

            Server.GetStaticData(this, ServerStaticDataCallback);
        }

        void ServerStaticDataCallback(long code, string compressedJson)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressedJson);

                RestoreStaticData(node);

                Utils.File.WriteJson(DataManager.LOCAL_STATIC_FILENAME, node);
            }

            else
            {
                if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localSaveJson))
                {
                    RestoreStaticData(JSON.Parse(localSaveJson));
                }

                else
                {
                    Utils.UI.ShowMessage("ServerError", "Server Connection", "A connection to the server is required.");

                    return;
                }
            }

            SceneManager.LoadSceneAsync("GameScene");
        }

        void RestoreStaticData(JSONNode node)
        {
            StaticData.Restore(node);
        }
    }
}