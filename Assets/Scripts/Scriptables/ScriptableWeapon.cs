using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Weapon")]
public class ScriptableWeapon : ScriptableObject
{
    public Sprite icon;

    public GameObject prefab;
}
