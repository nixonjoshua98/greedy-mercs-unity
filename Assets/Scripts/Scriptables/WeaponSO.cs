
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Scriptables.Weapons
{
    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/i/Weapon")]
    public class WeaponSO : ScriptableObject
    {
        public Sprite icon;
    }
}
