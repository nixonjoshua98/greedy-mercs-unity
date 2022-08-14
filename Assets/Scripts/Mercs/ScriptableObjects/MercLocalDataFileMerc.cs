using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using GM.Mercs.Data;

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
