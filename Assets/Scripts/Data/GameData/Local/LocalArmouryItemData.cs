using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GM.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArmouryItemData")]
    public class LocalArmouryItemData : ScriptableObject
    {
        public int Id;

        [Space]

        public Sprite Icon;
    }
}
