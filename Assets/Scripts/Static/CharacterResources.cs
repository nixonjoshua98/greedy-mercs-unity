
using System.Collections.Generic;

using UnityEngine;

public static class CharacterResources
{
    static readonly List<CharacterStaticData> Heroes = new List<CharacterStaticData>()
    {
        new CharacterStaticData(CharacterID.HERO_NUM_1,    "Lightning Wraith", "WraithLightning",        50),
        new CharacterStaticData(CharacterID.HERO_NUM_2,         "Stone Golem",      "GolemStone",        5_000),
        new CharacterStaticData(CharacterID.HERO_NUM_3,          "Fire Satyr",       "SatyrFire",        75_000),
        new CharacterStaticData(CharacterID.HERO_NUM_4,        "Fallen Angel",     "FallenAngel",        2_000_000),
        new CharacterStaticData(CharacterID.HERO_NUM_5,        "War Minotaur",        "Minotaur",        150_000_000)
    };

    public static CharacterStaticData GetCharacter(CharacterID hero)
    {
        foreach (CharacterStaticData info in Heroes)
        {
            if (info.HeroID == hero)
                return info;
        }

        return null;
    }

    public static GameObject GetHeroGameObject(CharacterID key) { return Resources.Load<GameObject>("Heroes/" + GetCharacter(key).GameObjectString); }


    public static bool GetNextHero(out CharacterStaticData hero)
    {
        hero = null;

        foreach (CharacterStaticData info in Heroes)
        {
            if (!GameState.Characters.TryGetHeroState(info.HeroID, out var _))
            {
                hero = info;

                return true;
            }
        }

        return false;
    }
}