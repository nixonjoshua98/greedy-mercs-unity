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
        Server.Login(this, LoginCallback);
    }

    void LoginCallback(long code, string json)
    {
        RestoreState(code, json);
    }

    void RestoreState(long code, string json)
    {
        bool isLocalSave = Utils.File.Read(DataManager.LOCAL_FILENAME, out string localSaveJson);

        // We have local data
        if (isLocalSave || code == 200)
        {
            GameState.Restore(isLocalSave ? localSaveJson : "{}");
           
            if (code == 200)
            {
                ServerPlayerData serverData = JsonUtility.FromJson<ServerPlayerData>(json);

                CompareHeroes(serverData);
            }
            
            SceneManager.LoadSceneAsync("GameScene");
        }

        else
        {
            ErrorText.text = "No internet connection";
        }
    }

    void CompareHeroes(ServerPlayerData serverPlayerData)
    {
        foreach (HeroState serverHero in serverPlayerData.heroes)
        {
            if (!GameState.TryGetHeroState(serverHero.heroId, out HeroState _))
                GameState.heroes.Add(serverHero);
        }

        Dictionary<HeroID, HeroState> serverHeroDataDict = serverPlayerData.GetHeroesAsDict();

        for (int i = 0; i < GameState.heroes.Count; ++i)
        {
            HeroState localHeroData = GameState.heroes[i];

            if (!serverHeroDataDict.TryGetValue(localHeroData.heroId, out HeroState _))
            {
                Debug.LogWarning("Removed hero '" + Enum.GetName(typeof(HeroID), localHeroData.heroId) + "' from local player data");

                GameState.heroes.RemoveAt(i);
            }
        }
    }
}
