
using UnityEngine;

namespace GM.Bounties
{
    [CreateAssetMenu(menuName = "Scriptables/LocalBountyData")]
    public class LocalBountyData : ScriptableObject
    {
        // 0_Bounty or 1_OgreMan or 2_Little_Jimmy
        public int ID => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name;

        public Sprite Icon;

        [Header("Objects")]
        public GameObject Prefab;

        [Space]

        public UI.BountySlot Slot;
    }
}
