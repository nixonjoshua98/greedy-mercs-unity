using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class LoginManager : MonoBehaviour
{
    [SerializeField] GameObject ServerConnectionNotice;

    void Awake()
    {
        StartGameStateRestore();
    }

    void ShowConnectionNotice()
    {
        Utils.UI.Instantiate(ServerConnectionNotice, Vector3.zero);
    }

    void StartGameStateRestore()
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

        Server.GetStaticData(this, ServerStaticDataCallback);
    }

    void ServerStaticDataCallback(long code, string json)
    {
        if (code == ServerCodes.OK)
        {
            ServerData.Restore(JSON.Parse(json));

            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, json);
        }

        else
        {
            if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localSaveJson))
                ServerData.Restore(JSON.Parse(localSaveJson));

            else
                ShowConnectionNotice();
        }

        SceneManager.LoadSceneAsync("GameScene");
    }
}
