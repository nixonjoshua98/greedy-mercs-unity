using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    [CreateAssetMenu(menuName = "Scriptables/MercDescription")]
    public class MercDescription : ScriptableObject
    {
        public MercID ID;

        public string Name = "<Missing Name>";

        public GameObject Prefab;
        public Sprite Icon;
    }
}
