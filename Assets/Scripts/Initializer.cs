using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

using BountyData;
using CharacterData;

public class Initializer : MonoBehaviour
{
    [SerializeField] GameObject ServerErrorMessage;

    [Header("Scriptables")]
    [SerializeField] BountyListSO BountyList;
    [SerializeField] CharacterListSO CharacterList;

    void Awake()
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
            JSONNode node = Utils.Json.Decode(compressed);

            GameState.Update(node);
        }

        else if (!isLocalSave && code != 200)
        {
            Utils.UI.ShowError(ServerErrorMessage, "Server Connection", "A connection to the server is required when playing for the first time");

            return;
        }

        Server.GetStaticData(this, ServerStaticDataCallback);
    }

    void ServerStaticDataCallback(long code, string compressedJson)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decode(compressedJson);

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
                Utils.UI.ShowError(ServerErrorMessage, "Server Connection", "A connection to the server is required.");

                return;
            }
        }

        SceneManager.LoadSceneAsync("GameScene");
    }

    void RestoreStaticData(JSONNode node)
    {
        BountyList.Restore(node["bounties"]);
        CharacterList.Restore(node["characters"]);

        StaticData.Restore(node);

        StaticData.AssignBounties(BountyList);
        StaticData.AssignCharacters(CharacterList);
    }
}