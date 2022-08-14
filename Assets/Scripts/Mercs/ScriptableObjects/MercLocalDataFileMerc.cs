using GM.Mercs.Data;
using UnityEngine;

namespace GM.Mercs.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/MercScriptableObject")]
    public class MercLocalDataFileMerc : ScriptableObject
    {
        public MercID ID;

        public GameObject Prefab;
        public Sprite Icon;
    }

    //public class MercLocalDataFile
    //{
    //    public List<MercLocalDataFileMerc> Mercs;
    //}
}
