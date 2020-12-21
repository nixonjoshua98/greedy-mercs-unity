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
        Server.Login(this, ServerLoginCallback);
    }

    void ShowConnectionNotice()
    {
        Utils.UI.Instantiate(ServerConnectionNotice, Vector3.zero);
    }

    void ServerLoginCallback(long code, string json)
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        // Online
        if (code == ServerCodes.OK)
        {
            GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"), JSON.Parse(json));

            Server.GetStaticData(this, ServerStaticDataCallback);
        }

        // Offline
        else
        {
            bool isLocalStatic = Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localStaticJson);

            if (isLocalStatic)
            {
                GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

                ServerData.Restore(JSON.Parse(localStaticJson)); // Load the locally stored static data

                SceneManager.LoadSceneAsync("GameScene");
            }

            else
            {
                ShowConnectionNotice();
            }
        }
    }

    void ServerStaticDataCallback(long code, string json)
    {
        // We are duplicating here but thats fine.


        if (code == ServerCodes.OK)
        {
            ServerData.Restore(JSON.Parse(json));

            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, json);

            SceneManager.LoadSceneAsync("GameScene");
        }

        else
        {
            ShowConnectionNotice();
        }
    }
}
