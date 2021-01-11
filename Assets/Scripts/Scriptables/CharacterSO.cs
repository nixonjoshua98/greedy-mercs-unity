using System.Collections.Generic;

using UnityEngine;

namespace CharacterData
{
    [System.Serializable]
    public class HeroPassiveUnlock
    {
        public int skill;

        public int unlockLevel;
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

        public ScriptableWeapon[] weapons;

        // - Runtime
        [HideInInspector] public int unlockOrder;
        [HideInInspector] public BigDouble purchaseCost;
        [HideInInspector] public List<HeroPassiveUnlock> Passives;

        public void OnAwake()
        {
            purchaseCost = BigDouble.Parse(purchaseCostString);
        }

        public void Init(int _unlockOrder, List<HeroPassiveUnlock> passives)
        {
            unlockOrder     = _unlockOrder;
            Passives        = passives;
        }
    }
}