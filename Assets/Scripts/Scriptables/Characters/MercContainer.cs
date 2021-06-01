using System.Collections.Generic;

using UnityEngine;

namespace GM.Characters
{    
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/MercContainer")]
    public class MercContainer : ScriptableObject
    {
        public CharacterID ID;

        public Sprite Icon;

        public GameObject Prefab;
    }
}