using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{    
    public enum LootID
    {
        CLICK_GUANTLET  = 0,
        IRON_SWORD      = 1,
        OLD_SHORTS      = 2,
        HP_POTION       = 3,
        LAST_RING       = 4,
        TAP_SCROLL      = 5,
        POWER_AXE       = 6,
        SPELL_TOME      = 7,
        BRACERS         = 8,
        LUCKY_GEM       = 9,
        JADE_NECKLACE   = 10,
        ARTEMIS_BOW     = 11,
        WEALTH_BAG      = 12,
        THIEF_BELT      = 13,
        CRIT_CLOAK      = 14,
        ENERGY_STONE    = 15,
        MAGIC_VIAL      = 16,
        RAW_MEAT        = 17,
        COOKED_MEAT     = 18,
        EXPLOSIVE_ARROW = 19,
        GOLD_ORE        = 20,
        GOLD_COINS      = 21,
        MIGHTY_FORK     = 22,
        EXTENSION_BELL  = 23,
        NO_MIGHTY_SPOON = 24,
        METEORITE       = 25,
    }

    public enum ValueType
    {
        MULTIPLY            = 0,
        ADDITIVE_PERCENT    = 1,
        ADDITIVE_FLAT_VAL   = 2
    }


    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/LootItem")]
    public class LootItemSO : ScriptableObject
    {
        public LootID ItemID;

        public new string name;

        public string description;

        public Sprite icon;

        [HideInInspector] public BonusType bonusType;
        [HideInInspector] public ValueType valueType;

        [HideInInspector] public int maxLevel;

        [HideInInspector] public float costExpo;
        [HideInInspector] public float costCoeff;
        [HideInInspector] public float baseEffect;
        [HideInInspector] public float levelEffect;

        public void Init(LootStaticData data)
        {
            bonusType = data.bonusType;
            valueType = data.valueType;

            maxLevel = data.maxLevel;

            costExpo = data.costExpo;
            costCoeff = data.costCoeff;

            baseEffect = data.baseEffect;
            levelEffect = data.levelEffect;
        }
    }
}