
using UnityEngine;

namespace BountyData
{
    public enum BountyID
    {
        BOUNTY_ONE      = 0,
        BOUNTY_TWO      = 1,
        BOUNTY_THREE    = 2,
        BOUNTY_FOUR     = 3,
        BOUNTY_GIVE     = 4
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Bounty")]
    public class BountySO : ScriptableObject
    {
        public BountyID BountyID;

        [Space]

        public new string name;

        public Sprite icon;

        public GameObject prefab;

        [Header("Runtime")]
        public int bountyPoints;
        public int unlockStage;

        public void Init(BountyStaticData data)
        {
            bountyPoints = data.bountyPoints;
            unlockStage = data.unlockStage;
        }
    }
}