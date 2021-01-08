using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RelicData;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Relic")]
public class ScriptableRelic : ScriptableObject
{
    public RelicID relic;

    public new string name;

    public string description;

    public Sprite icon;

    [HideInInspector] public RelicStaticData data;

    public void Init(RelicStaticData _data)
    {
        data = _data;
    }
}
