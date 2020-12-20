using System;
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
    static readonly Dictionary<HeroID, string> gameObjectLookup = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" },
        { HeroID.FALLEN_ANGEL,      "FallenAngel" }
    };

    public static string GetGameObjectString(HeroID key) { return gameObjectLookup[key]; }

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
                return "{Missing}";
        }
    }
}