﻿using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class LoginManager : MonoBehaviour
{
    [SerializeField] GameObject ServerErrorMessage;

    void Awake()
    {
        Debug.Log(Application.persistentDataPath);

        StartGameStateRestore();
    }

    void StartGameStateRestore()
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        GameState.Restore(JSON.Parse(isLocalSave ? localSaveJson : "{}"));

        // === Request ===

        Server.Login(this, ServerLoginCallback, Utils.Json.GetDeviceNode());
    }

    void ServerLoginCallback(long code, string compressedJson)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(compressedJson);

            GameState.Update(node);
        }

        Server.GetStaticData(this, ServerStaticDataCallback);
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
