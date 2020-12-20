using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginManager : MonoBehaviour
{
    [SerializeField] Text ErrorText;

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

        GameState.Restore(isLocalSave ? localSaveJson : "{}");

        // Online
        if (code == ServerCodes.OK)
        {
            CompareHeroes(JsonUtility.FromJson<ServerPlayerData>(json));

            Server.GetStaticData(this, ServerStaticDataCallback);
        }

        // Offline
        else
        {
            bool isLocalStatic = Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localStaticJson);

            if (isLocalStatic)
            {
                ServerData.Restore(localStaticJson);

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
            ServerData.Restore(json);

            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, json);

            SceneManager.LoadSceneAsync("GameScene");
        }

        else
        {
            ShowConnectionNotice();
        }
    }

    void CompareHeroes(ServerPlayerData serverPlayerData)
    {
        // Add heroes from server which are not in the local game state
        foreach (HeroState serverHero in serverPlayerData.heroes)
        {
            if (!GameState.TryGetHeroState(serverHero.heroId, out HeroState _))
                GameState.heroes.Add(serverHero);
        }

        Dictionary<HeroID, HeroState> serverHeroDataDict = serverPlayerData.GetHeroesAsDict();

        for (int i = 0; i < GameState.heroes.Count;)
        {
            HeroState localHeroData = GameState.heroes[i];

            // Player has a hero which is not on the server
            if (!serverHeroDataDict.TryGetValue(localHeroData.heroId, out HeroState _))
            {
                GameState.heroes.RemoveAt(i);

                // Probably editing the local game file save
            }

            else
            {
                ++i;
            }
        }
    }
}
