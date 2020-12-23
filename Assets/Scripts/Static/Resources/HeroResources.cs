
using System.Collections.Generic;

using UnityEngine;

public enum HeroID
{
    HERO_NUM_1    = 0,
    HERO_NUM_2    = 1,
    HERO_NUM_3    = 2,
    HERO_NUM_4    = 3,
    HERO_NUM_5    = 4,
}

public enum BonusType
{
    ALL_SQUAD_DAMAGE    = 0,
    ENEMY_GOLD          = 1,
    TAP_DAMAGE          = 2,
    BOSS_GOLD           = 3
}


public static class HeroResources
{
    static readonly List<HeroStaticData> Heroes = new List<HeroStaticData>()
    {
        new HeroStaticData(HeroID.HERO_NUM_1,    "Lightning Wraith", "WraithLightning",        50),
        new HeroStaticData(HeroID.HERO_NUM_2,         "Stone Golem",      "GolemStone",        5_000),
        new HeroStaticData(HeroID.HERO_NUM_3,          "Fire Satyr",       "SatyrFire",        75_000),
        new HeroStaticData(HeroID.HERO_NUM_4,        "Fallen Angel",     "FallenAngel",        2_000_000),
        new HeroStaticData(HeroID.HERO_NUM_5,        "War Minotaur",        "Minotaur",        150_000_000)
    };

    public static HeroStaticData GetHero(HeroID hero)
    {
        foreach (HeroStaticData info in Heroes)
        {
            if (info.HeroID == hero)
                return info;
        }

        return null;
    }

    public static GameObject GetHeroGameObject(HeroID key) { return Resources.Load<GameObject>("Heroes/" + GetHero(key).GameObjectString); }


    public static bool GetNextHero(out HeroStaticData hero)
    {
        hero = null;

        foreach (HeroStaticData info in Heroes)
        {
            if (!GameState.TryGetHeroState(info.HeroID, out var _))
            {
                hero = info;

                return true;
            }
        }

        return false;
    }

    public static string PassiveTypeToString(BonusType skill)
    {
        switch (skill)
        {
            case BonusType.ALL_SQUAD_DAMAGE:    return "squad damage";
            case BonusType.TAP_DAMAGE:          return "tap damage";
            case BonusType.ENEMY_GOLD:          return "enemy gold";
            case BonusType.BOSS_GOLD:           return "boss gold";

            default:
                return "<error>";
        }
    }
}