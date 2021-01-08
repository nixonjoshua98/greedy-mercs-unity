
using UnityEngine;

using ServerBountyData = BountyData.ServerBountyData;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Bounty")]
public class BountySO : ScriptableObject
{
    public int BountyID;

    [Space]

    public new string name;

    public Sprite icon;

    public GameObject prefab;

    [HideInInspector] public int bountyPoints;
    [HideInInspector] public int unlockStage;

    public void OnAwake()
    {
        ServerBountyData data = StaticData.Bounties.Get(BountyID);

        bountyPoints    = data.bountyPoints;
        unlockStage     = data.unlockStage;
    }
}