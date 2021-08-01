using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{

    public enum ItemType
    {
        BLUE_GEM = 100,
        ARMOURY_POINTS = 200,
        PRESTIGE_POINTS = 300,
        BOUNTY_POINTS = 400
    }


    [CreateAssetMenu(menuName = "Scriptables/LocalItemData")]
    public class LocalItemData : ScriptableObject
    {
        public ItemType Item;
        public Sprite Icon;

        public string DisplayName;
    }
}
