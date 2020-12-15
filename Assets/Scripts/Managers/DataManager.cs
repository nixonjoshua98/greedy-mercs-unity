using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ServerPlayerData
{
    public List<HeroState> heroes;

    // === Helper Methods ===
    public Dictionary<HeroID, HeroState> GetHeroesAsDict()
    {
        Dictionary<HeroID, HeroState> heroesDict = new Dictionary<HeroID, HeroState>();

        foreach (HeroState HeroState in heroes)
            heroesDict[HeroState.heroId] = HeroState;

        return heroesDict;
    }
}

[System.Serializable]
public class HeroState
{
    public HeroID heroId = HeroID.ERROR;

    public int level = 1;

    public int dummyValue = -1;

    public bool inSquad = false;
}


[System.Serializable]
public class PlayerState
{
    public double gold = 0;
}


[System.Serializable]
public class GameState
{
    static GameStateStruct State = null;

    class GameStateStruct
    {
        public PlayerState player;

        public List<HeroState> heroes;
    }

    public static PlayerState gold { get { return State.player; } }
    public static List<HeroState> heroes { get { return State.heroes; } }

    public static void RestoreFromJson(string json)
    {
        if (State == null)
            State = JsonUtility.FromJson<GameStateStruct>(json);
    }

    public static string ToJson()
    {
        return JsonUtility.ToJson(State);
    }

    // === Helper Methods ===
    static HeroState GetHeroState(HeroID heroId)
    {
        return State.heroes.Find(ele => ele.heroId == heroId);
    }
}

public class DataManager : MonoBehaviour
{
    void Start()
    {
        RestoreState();
    }

    public static ServerPlayerData GetServerPlayerData()
    {
        return JsonUtility.FromJson<ServerPlayerData>("{\"heroes\": [{\"heroId\": 10000, \"dummyValue\": 0}]}");
    }

    public static void RestoreState()
    {
        GameState.RestoreFromJson(Utils.File.Read("localsave"));

        ServerPlayerData serverData = GetServerPlayerData();

        Dictionary<HeroID, HeroState> serverHeroDataDict = serverData.GetHeroesAsDict();

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
