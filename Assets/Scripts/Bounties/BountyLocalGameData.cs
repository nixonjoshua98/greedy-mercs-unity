
using UnityEngine;

namespace GM.Bounties.ScripableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/BountyLocalGameData")]
    public class BountyLocalGameData : ScriptableObject
    {
        // 0_Bounty or 1_OgreMan or 2_Little_Jimmy
        public int Id => int.Parse(name.Split('_')[0]);

        [Space]

        public Sprite Icon;

        [Header("Objects")]
        public GameObject Prefab;
    }
}
