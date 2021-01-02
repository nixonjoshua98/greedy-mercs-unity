using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;

[CreateAssetMenu(menuName ="Scriptables/Character")]
public class ScriptableCharacter : ScriptableObject
{
    public CharacterID character;

    // Value is assigned to in CharacterResources
    [HideInInspector] public int unlockOrder;

    public new string name;

    public BonusType attackType;

    public double purchaseCost;

    public GameObject prefab;

    public Sprite icon;
}
