
using UnityEngine;

namespace GreedyMercs
{
    public enum BountyID
    {
        BOUNTY_ONE      = 0,
        BOUNTY_TWO      = 1,
        BOUNTY_THREE    = 2,
        BOUNTY_FOUR     = 3,
        BOUNTY_FIVE     = 4
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Bounty")]
    public class BountySO : ScriptableObject
    {
        public BountyID BountyID;

        [Space]

        public new string name;

        public string iconString;
        public string prefabString;

        public Sprite icon;

        public GameObject prefab;

        [HideInInspector] public int hourlyIncome;
        [HideInInspector] public int unlockStage;
        [HideInInspector] public int maxLevel;

        public void Init(BountyServerData data)
        {
            hourlyIncome = data.hourlyIncome;
            unlockStage = data.unlockStage;
            maxLevel = data.maxLevel;

            iconString      = data.iconString;
            prefabString    = data.prefabString;
        }
    }
}