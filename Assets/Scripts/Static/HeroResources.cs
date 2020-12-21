using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum HeroID
{
    ERROR = -1,

    WRAITH_LIGHTNING    = 0,
    GOLEM_STONE         = 1,
    SATYR_FIRE          = 2,
    FALLEN_ANGEL        = 3
}

public enum BonusType
{
    ERROR = -1,

    ALL_SQUAD_DAMAGE    = 0,
    ENEMY_GOLD          = 1
}


public static class HeroResources
{
    static readonly Dictionary<HeroID, string> GameObjectDict = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" },
        { HeroID.FALLEN_ANGEL,      "FallenAngel" }
    };

    static readonly Dictionary<int, HeroID> HeroUnlocks = new Dictionary<int, HeroID>()
    {
        {1,     HeroID.WRAITH_LIGHTNING },
        {10,    HeroID.GOLEM_STONE },
        {35,    HeroID.SATYR_FIRE },
        {75,    HeroID.FALLEN_ANGEL },
    };

    public static string GetGameObjectString(HeroID key) { return GameObjectDict[key]; }

    public static bool GetNextHeroUnlock(out int stage, out HeroID hero)
    {
        stage = 0;
        hero = HeroID.ERROR;

        List<int> values = HeroUnlocks.Keys.ToList();

        values.Sort();

        foreach (int stageUnlock in values)
        {
            if (!GameState.TryGetHeroState(HeroUnlocks[stageUnlock], out var _) || stageUnlock > GameState.stage.stage)
            {
                stage = stageUnlock;

                hero = HeroUnlocks[stageUnlock];

                return true;
            }
        }

        return false;
    }

    public static GameObject GetHeroGameObject(HeroID key)
    {
        return Resources.Load<GameObject>("Heroes/" + GetGameObjectString(key));
    }

    public static string PassiveTypeToString(BonusType skill)
    {
        switch (skill)
        {
            case BonusType.ALL_SQUAD_DAMAGE:
                return "squad damage";

            case BonusType.ENEMY_GOLD:
                return "enemy gold";

            default:
                return "{missing}";
        }
    }
}