using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GreedyMercs.Armoury.Data
{
    public enum WeaponType
    {
        SWORD = 0
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/ArmouryWeapon")]
    public class ArmouryWeaponSO : ScriptableObject
    {
        public WeaponType Type;

        public Sprite icon;
    }
}