using System;
using System.Collections.Generic;
using UnityEngine;

namespace SRC.Bounties.Scriptables
{
    [Serializable]
    public class BountiesLocalDataFileBounty
    {
        public int BountyID;
        public Sprite Icon;
        public GameObject Prefab;
    }

    [CreateAssetMenu(menuName = "Scriptables/BountiesLocalDataFile")]
    public class BountiesLocalDataFile : ScriptableObject
    {
        public List<BountiesLocalDataFileBounty> Bounties;
    }
}
