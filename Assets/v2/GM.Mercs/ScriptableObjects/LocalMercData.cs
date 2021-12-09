using MercID = GM.Common.Enums.MercID;
using AttackType = GM.Common.Enums.AttackType;

using UnityEngine;

namespace GM.Mercs.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/LocalMercData")]
    public class LocalMercData : ScriptableObject
    {
        public MercID ID;
        public AttackType AttackType = AttackType.MELEE;

        public string Name = "<Missing Name>";

        public GameObject Prefab;
        public Sprite Icon;
    }
}
