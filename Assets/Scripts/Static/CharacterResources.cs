
using System.Collections.Generic;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public static class CharacterResources
{
    static readonly List<CharacterStaticData> Heroes = new List<CharacterStaticData>()
    {
        new CharacterStaticData(CharacterID.HERO_NUM_1,    BonusType.MAGE_DAMAGE, "Lightning Wraith", "WraithLightning",       50),
        new CharacterStaticData(CharacterID.HERO_NUM_2,    BonusType.MELEE_DAMAGE,     "Stone Golem",      "GolemStone",       5_000),
        new CharacterStaticData(CharacterID.HERO_NUM_3,    BonusType.MAGE_DAMAGE,       "Fire Satyr",       "SatyrFire",       75_000),
        new CharacterStaticData(CharacterID.HERO_NUM_4,    BonusType.MELEE_DAMAGE,    "Fallen Angel",     "FallenAngel",       2_000_000),
        new CharacterStaticData(CharacterID.HERO_NUM_5,    BonusType.MELEE_DAMAGE,    "War Minotaur",        "Minotaur",       125_000_000),
        new CharacterStaticData(CharacterID.HERO_NUM_6,    BonusType.MELEE_DAMAGE,      "Reaper Man",       "ReaperMan",       4_500_000_000_000)
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