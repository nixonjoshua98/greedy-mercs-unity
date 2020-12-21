using System;
using System.Collections.Generic;

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

        Server.GetStaticData(this, ServerStaticDataCallback);
    }

    void ServerStaticDataCallback(long code, string json)
    {
        if (code == 200)
        {
            ServerData.Restore(JSON.Parse(json));

            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, json);
        }

        else
        {
            if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localSaveJson))
                ServerData.Restore(JSON.Parse(localSaveJson));

            else
                Utils.UI.ShowError(ServerErrorMessage, "Server Connection", "A connection to the server is required when playing for the first time");
        }

        SceneManager.LoadSceneAsync("GameScene");
    }
}
