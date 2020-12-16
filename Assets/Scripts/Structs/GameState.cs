using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ServerPlayerData
{
    public List<HeroState> heroes = new List<HeroState>();

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
    static _GameState State = null;

    class _GameState
    {
        public PlayerState player = new PlayerState();

        public List<HeroState> heroes = new List<HeroState>();

        public StageData stage;
    }

    public static PlayerState player { get { return State.player; } }
    public static StageData stage { get { return State.stage; } }
    public static List<HeroState> heroes { get { return State.heroes; } }

    public static void Restore(string json)
    {
        if (State == null)
            State = JsonUtility.FromJson<_GameState>(json);
    }

    public static string ToJson()
    {
        return JsonUtility.ToJson(State);
    }

    // === Helper Methods ===
    public static HeroState GetHeroState(HeroID heroId)
    {
        return State.heroes.Find(ele => ele.heroId == heroId);
    }

    public static bool TryGetHeroState(HeroID heroId, out HeroState result)
    {
        result = GetHeroState(heroId);

        return result != null;
    }
}