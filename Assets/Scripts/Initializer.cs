using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [SerializeField] GameObject ServerErrorMessage;

    void Awake()
    {
        Debug.Log(Application.persistentDataPath);

        DataManager.IsPaused = false;

        StatsCache.Clear();

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

            StaticData.Restore(node);

            Utils.File.WriteJson(DataManager.LOCAL_STATIC_FILENAME, node);
        }

        else
        {
            if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localSaveJson))
                StaticData.Restore(JSON.Parse(localSaveJson));

            else
            {
                Utils.UI.ShowError(ServerErrorMessage, "Server Connection", "A connection to the server is required when playing for the first time");

                return;
            }
        }

        SceneManager.LoadSceneAsync("GameScene");
    }
}
