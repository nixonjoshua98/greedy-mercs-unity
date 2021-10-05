
using UnityEngine;

namespace GM.Mercs.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalMercData")]
    public class LocalMercData : ScriptableObject
    {
        public MercID ID;

        public string Name = "<Missing Name>";

        public GameObject Prefab;
        public Sprite Icon;
    }
}
