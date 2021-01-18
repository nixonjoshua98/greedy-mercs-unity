
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Armoury.UI
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/i/WeaponList")]
    public class WeaponListSO : ScriptableObject
    {
        [SerializeField] WeaponSO[] WeaponArray;

        public int Count { get { return WeaponArray.Length; } }

        public WeaponSO Get(int index) => WeaponArray[index];
    }
}