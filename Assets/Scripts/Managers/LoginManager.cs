using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class LoginManager : MonoBehaviour
{
    [SerializeField] GameObject ServerErrorMessage;

    void Awake()
    {
        StartGameStateRestore();
    }

    void StartGameStateRestore()
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

        Server.Login(this, ServerLoginCallback);
    }

    void ServerLoginCallback(long code, string compressedJson)
    {
        Server.GetStaticData(this, ServerStaticDataCallback);
    }

    void ServerStaticDataCallback(long code, string compressedJson)
    {
        string json = Utils.GZip.Unzip(Convert.FromBase64String(compressedJson));

        if (code == 200)
        {
            StaticData.Restore(JSON.Parse(json));

            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, json);
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
