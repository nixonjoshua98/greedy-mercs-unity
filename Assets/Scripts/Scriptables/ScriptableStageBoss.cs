using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/NamedBoss")]
public class ScriptableStageBoss : ScriptableObject
{
    public new string name;

    public GameObject prefab;
}