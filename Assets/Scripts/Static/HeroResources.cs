using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum HeroID
{
    ERROR = -1,

    WRAITH_LIGHTNING    = 10_000,
    GOLEM_STONE         = 10_001,
    SATYR_FIRE          = 10_002,
    FALLEN_ANGEL        = 10_003
}

public enum PassiveSkillID
{ 
    ERROR = -1,

    DOUBLE_SQUAD_DAMAGE = 11_000,
}

public enum PassiveSkillType
{
    ERROR = -1,

    SQUAD_DAMAGE = 0,
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

    public static string PassiveTypeToString(PassiveSkillType skill)
    {
        switch (skill)
        {
            case PassiveSkillType.SQUAD_DAMAGE:
                return "squad damage";

            default:
                return "{SkillName}";
        }
    }
}