using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Bounty
{
    [CreateAssetMenu(menuName = "Scriptables/LocalBountyData")]
    public class LocalBountyData : ScriptableObject
    {
        // 0_Bounty or 1_OgreMan or 2_Little_Jimmy
        public int Id => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name;

        public GameObject Prefab;

        public Sprite Icon;
    }
}
