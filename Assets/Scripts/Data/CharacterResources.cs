
using System.Collections.Generic;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public static class CharacterResources
{
    static readonly List<CharacterStaticData> Heroes = new List<CharacterStaticData>()
    {
        new CharacterStaticData(CharacterID.WRAITH,    BonusType.MAGE_DAMAGE, "Lightning Wraith", "WraithLightning",       50),
        new CharacterStaticData(CharacterID.GOLEM,    BonusType.MELEE_DAMAGE,     "Stone Golem",      "GolemStone",       5_000),
        new CharacterStaticData(CharacterID.SATYR,    BonusType.MAGE_DAMAGE,       "Fire Satyr",       "SatyrFire",       75_000),
        new CharacterStaticData(CharacterID.ANGEL,    BonusType.MELEE_DAMAGE,    "Fallen Angel",     "FallenAngel",       2_000_000),
        new CharacterStaticData(CharacterID.MINOTAUR,    BonusType.MELEE_DAMAGE,    "War Minotaur",        "Minotaur",       125_000_000),
        new CharacterStaticData(CharacterID.REAPER,    BonusType.MELEE_DAMAGE,      "Reaper Man",       "ReaperMan",       4_500_000_000_000)
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