using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// === Local Player Data
#region local


[System.Serializable]
public class PlayerState
{
    public double gold = 0;

    public int maxStageReward = 100_000;
}


[System.Serializable]
public class HeroState
{
    public HeroID heroId = HeroID.ERROR;

    public int level = 1;

    public bool inSquad = false;
}


#endregion

// === Static Data ===

#region static

[System.Serializable]
public class PlayerStageReward
{
    public HeroID heroId = HeroID.ERROR;
}


[System.Serializable]
public class HeroPassiveUnlock
{
    public int skill = -1;

    public int unlockLevel = 100_000;
}


[System.Serializable]
public class StaticServerHeroData
{
    public float baseCost = 1;
}


[System.Serializable]
public class HeroPassiveSkill
{
    public string name = "No Name";

    public double value = 1;

    public BonusType type = BonusType.ERROR;
}


#endregion

// === Server Player Data

#region server


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

#endregion