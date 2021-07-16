using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalBountyData")]
    public class LocalBountyData : ScriptableObject
    {
        public int Id;

        [Space]

        public string Name;

        public GameObject Prefab;

        public Sprite Icon;
    }
}
