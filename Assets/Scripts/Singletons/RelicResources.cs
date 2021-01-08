using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RelicData;

public class RelicResources : MonoBehaviour
{
    static RelicResources Instance = null;

    // === Data ===
    public List<ScriptableRelic> Relics;

    void Awake()
    {
        Instance = this;

        foreach (var r in StaticData.Relics.All)
        {
            Get(r.Key).Init(r.Value);
        }
    }

    public static ScriptableRelic Get(RelicID relic)
    {
        foreach (ScriptableRelic r in Instance.Relics)
        {
            if (r.relic == relic)
                return r;
        }

        return null;
    }
}
