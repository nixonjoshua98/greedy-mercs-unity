
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Bounty")]
public class BountySO : ScriptableObject
{
    public BountyID BountyID;

    [Space]

    public new string name;

    public Sprite icon;

    public GameObject prefab;

    [HideInInspector] public int bountyPoints;
    [HideInInspector] public int unlockStage;

    public void Init(BountyStaticData data)
    {
        bountyPoints    = data.bountyPoints;
        unlockStage     = data.unlockStage;
    }
}