using System.Collections.Generic;

using UnityEngine;

namespace Data.Characters
{    
    public enum CharacterID
    {
        WRAITH      = 0,
        GOLEM       = 1,
        SATYR       = 2,
        ANGEL       = 3,
        MINOTAUR    = 4,
        REAPER      = 5,
        FIRE_GOLEM  = 6
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Character")]
    public class CharacterSO : ScriptableObject
    {
        public CharacterID CharacterID;

        public new string name;

        public BonusType attackType;

        [SerializeField] string purchaseCostString;

        public GameObject prefab;

        public Sprite icon;

        public WeaponSO[] weapons;

        // - Runtime
        [HideInInspector] public int unlockOrder;
        [HideInInspector] public BigDouble purchaseCost;
        [HideInInspector] public List<CharacterPassive> passives;

        public void Init(int _unlockOrder, List<CharacterPassive> _passives)
        {
            purchaseCost = BigDouble.Parse(purchaseCostString);

            unlockOrder = _unlockOrder;
            passives    = _passives;
        }
    }
}