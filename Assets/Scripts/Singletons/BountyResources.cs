using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyResources : MonoBehaviour
{
    static BountyResources Instance = null;

    // === Data ===
    public List<ScriptableBounty> bounties;

    public static List<ScriptableBounty> All { get { return Instance.bounties; } }

    void Awake()
    {
        Instance = this;

        foreach (var bounty in StaticData.Bounties.All())
        {
            bounties[bounty.Key].Init(bounty.Value);
        }
    }

    public static ScriptableBounty Get(int index)
    {
        return Instance.bounties[index];
    }

    public static bool TryGetStageBoss(int stage, out ScriptableBounty bounty)
    {
        bounty = null;

        foreach (var b in All)
        {
            if (b.unlockStage == stage)
            {
                bounty = b;

                return true;
            }
        }

        return false;
    }
}