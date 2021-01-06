using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "Scriptables/Character")]
public class ScriptableCharacter : ScriptableObject
{
    public CharacterID character;

    // Value is assigned to in CharacterResources
    [HideInInspector] public int unlockOrder;

    public new string name;

    public BonusType attackType;

    public string purchaseCostString;

    public GameObject prefab;

    public Sprite icon;

    public ScriptableWeapon[] weapons;

    // Set at Awake
    [HideInInspector] public BigDouble purchaseCost;

    void Awake()
    {
        purchaseCost = BigDouble.Parse(purchaseCostString);
    }
}
