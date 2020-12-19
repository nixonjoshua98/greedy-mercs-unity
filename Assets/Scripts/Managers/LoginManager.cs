using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] Text ErrorText;

    void Awake()
    {
        Server.Login(this, ServerLoginCallback);
    }

    void ServerLoginCallback(long code, string json)
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        // We have local data
        if (isLocalSave || code == 200)
        {
            GameState.Restore(isLocalSave ? localSaveJson : "{}");

            if (GameState.IsValid())
            {
                Server.GetStaticData(this, ServerStaticDataCallback);

                if (code == 200)
                {
                    ServerPlayerData serverData = JsonUtility.FromJson<ServerPlayerData>(json);

                    CompareHeroes(serverData);

                }
            }

            else
                ErrorText.text = "Game state failed to restore";
        }
    }

    void ServerStaticDataCallback(long code, string json)
    {
        if (code == 200)
        {
            ServerData.Restore(json);
        }

        else if (Utils.File.Read(DataManager.LOCAL_STATIC_FILENAME, out string localStaticJson))
        {
            ServerData.Restore(localStaticJson);
        }

        // ===

        if (ServerData.IsValid())
        {
            Utils.File.Write(DataManager.LOCAL_STATIC_FILENAME, ServerData.ToJson());

            SceneManager.LoadSceneAsync("GameScene");
        }

        else
            ErrorText.text = "Static data failed to restore";
    }

    void CompareHeroes(ServerPlayerData serverPlayerData)
    {
        // Add 
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
            }

            else
            {
                ++i;
            }
        }
    }
}
