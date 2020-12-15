using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    static DataManager Instance = null;

    [SerializeField] bool loadSceneAfterLoading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Deleted duplicated DataManager instance.");

            Destroy(this);
        }
    }

    void Start()
    {
        Server.Login(this, LoginCallback);
    }

    void WriteStateToFile()
    {
        Debug.Log("Saved to file");

        Utils.File.Write("localsave", GameState.ToJson());
    }

    void LoginCallback(long code, string json)
    {
        RestoreState(code, json);
    }

    void RestoreState(long code, string json)
    {
        GameState.RestoreFromJson(Utils.File.Read("localsave", out string content) ? content : "{}");

        if (code == 200)
        {
            ServerPlayerData serverPlayerData = JsonUtility.FromJson<ServerPlayerData>(json);

            CompareHeroes(serverPlayerData);

            CompareHeroAttributes(serverPlayerData);
        }

        if (loadSceneAfterLoading)
            SceneManager.LoadSceneAsync("GameScene");

        InvokeRepeating("WriteStateToFile", 10.0f, 5.0f);
    }

    void CompareHeroes(ServerPlayerData serverPlayerData)
    {
        // Add heroes which exist on the server, but not locally
        foreach (HeroState serverHero in serverPlayerData.heroes)
        {
            if (!GameState.TryGetHeroState(serverHero.heroId, out HeroState _))
            {
                GameState.heroes.Add(serverHero);
            }
        }
    }

    void CompareHeroAttributes(ServerPlayerData serverPlayerData)
    {
        Dictionary<HeroID, HeroState> serverHeroDataDict = serverPlayerData.GetHeroesAsDict();

        for (int i = 0; i < GameState.heroes.Count; ++i)
        {
            HeroState localHeroData = GameState.heroes[i];

            // Override local data with the data pulled from the server
            if (serverHeroDataDict.TryGetValue(localHeroData.heroId, out HeroState serverHeroData))
            {
                localHeroData.dummyValue = serverHeroData.dummyValue;
            }

            // Player has a hero which should not have (or there is a bug)
            else
            {
                Debug.LogWarning("Player has hero '" + Enum.GetName(typeof(HeroID), localHeroData.heroId) + "' in local data while not in server data");
            }
        }
    }
}
