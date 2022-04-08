using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/MercScriptableObject")]
    public class MercScriptableObject : ScriptableObject
    {
        public MercID ID;

        public GameObject Prefab;
        public Sprite Icon;
    }
}
