using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    public Sprite icon;

    public GameObject projectile;
}
