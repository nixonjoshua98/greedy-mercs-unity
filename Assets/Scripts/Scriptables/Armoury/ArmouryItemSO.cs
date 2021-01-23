using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GreedyMercs.Armoury.Data
{
    public enum WeaponType
    {
        SWORD   = 0,
        STAFF   = 1
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/ArmouryItem")]
    public class ArmouryItemSO : ScriptableObject
    {
        public WeaponType Type;

        public Sprite icon;

        [Header("Runtime")]
        public int Index;

        [Header("Static Data")]
        public double damageBonus = 1;

        public void Init(int index, JSONNode node)
        {
            Index = index;

            damageBonus = node["damageBonus"].AsDouble;
        }
    }
}