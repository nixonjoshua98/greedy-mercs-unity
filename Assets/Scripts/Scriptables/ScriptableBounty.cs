using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BountyData;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Bounty")]
public class ScriptableBounty : ScriptableObject
{
    public new string name;

    public Sprite icon;

    public GameObject prefab;

    // Pulled from the static data
    [HideInInspector] public int bountyPoints;
    [HideInInspector] public int unlockStage;

    public void Init(BountyStaticData data)
    {
        bountyPoints    = data.bountyPoints;
        unlockStage     = data.unlockStage;
    }
}
