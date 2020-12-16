using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum HeroID
{
    ERROR = -1,

    // Note: Heroes will have the prefix 10
    WRAITH_LIGHTNING    = 10_000,
    GOLEM_STONE         = 10_001,
    SATYR_FIRE          = 10_002
}

public static class HeroResources
{
    static readonly Dictionary<HeroID, string> gameObjectLookup = new Dictionary<HeroID, string>()
    {
        { HeroID.WRAITH_LIGHTNING,  "WraithLightning" },
        { HeroID.GOLEM_STONE,       "GolemStone" },
        { HeroID.SATYR_FIRE,        "SatyrFire" }
    };

    public static string GetGameObjectString(HeroID key) { return gameObjectLookup[key]; }

    public static GameObject GetHeroGameObject(HeroID key)
    {
        return Resources.Load<GameObject>("Heroes/" + GetGameObjectString(key));
    }
}