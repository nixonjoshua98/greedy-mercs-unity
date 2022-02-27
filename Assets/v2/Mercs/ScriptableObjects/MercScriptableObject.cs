using UnitID = GM.Common.Enums.UnitID;
using AttackType = GM.Common.Enums.AttackType;

using UnityEngine;

namespace GM.Mercs.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/MercScriptableObject")]
    public class MercScriptableObject : ScriptableObject
    {
        public UnitID ID;
        public AttackType AttackType = AttackType.MELEE;

        public string Name = "???";

        public GameObject Prefab;
        public Sprite Icon;
    }
}
