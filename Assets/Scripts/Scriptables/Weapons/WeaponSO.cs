
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Weapon")]
public class WeaponSO : ScriptableObject
{
    public Sprite icon;

    [Header("Optional")]
    public GameObject prefab;
}