using MercID = GM.Common.Enums.MercID;
using AttackType = GM.Common.Enums.AttackType;

using UnityEngine;

namespace GM.Mercs.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/MercScriptableObject")]
    public class MercScriptableObject : ScriptableObject
    {
        public MercID ID;
        public AttackType AttackType = AttackType.MELEE;

        public string Name = "???";

        public GameObject Prefab;
        public Sprite Icon;
    }
}
