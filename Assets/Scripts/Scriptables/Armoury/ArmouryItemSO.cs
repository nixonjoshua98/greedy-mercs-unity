using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GreedyMercs.Armoury.Data
{
    public enum WeaponType
    {
        SWORD   = 0,
        STAFF   = 1,
        AXE     = 2
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/ArmouryItem")]
    public class ArmouryItemSO : ScriptableObject
    {
        public WeaponType Type;

        public Sprite icon;

        [Header("Static Data")]
        public int ItemID;

        [Space]
        public int starRating;
        public int upgradeCost;
        public int evoUpgradeCost;

        [Space]
        public double damageBonus;

        public void Init(int index, JSONNode node)
        {
            ItemID = index;

            evoUpgradeCost = 10;

            damageBonus = node["damageBonus"].AsDouble;

            starRating  = node["itemTier"].AsInt;
            upgradeCost = node["upgradeCost"].AsInt;
        }
    }
}