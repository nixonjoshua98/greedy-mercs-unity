using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{    
    public enum CharacterID { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Character")]
    public class CharacterSO : ScriptableObject
    {
        public CharacterID CharacterID;

        public new string name;

        public BonusType attackType;

        public GameObject prefab;

        public Sprite icon;

        // - Runtime
        [HideInInspector] public int unlockOrder;
        [HideInInspector] public BigDouble baseDamage;
        [HideInInspector] public BigDouble unlockCost;
        [HideInInspector] public List<CharacterPassive> passives;

        public void Init(int _unlockOrder, string _unlockCost, string _baseDamage, List<CharacterPassive> _passives)
        {
            baseDamage  = BigDouble.Parse(_baseDamage == "" ? "0" : _baseDamage);
            unlockCost  = BigDouble.Parse(_unlockCost);

            unlockOrder = _unlockOrder;
            passives    = _passives;
        }
    }
}