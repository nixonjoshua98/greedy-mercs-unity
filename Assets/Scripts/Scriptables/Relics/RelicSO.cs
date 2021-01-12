using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RelicData;

namespace RelicData
{    public enum RelicID
    {
        CLICK_GUANTLET = 0,
        IRON_SWORD = 1,
        OLD_SHORTS = 2,
        HP_POTION = 3,
        LAST_RING = 4,
        TAP_SCROLL = 5,
        POWER_AXE = 6,
        SPELL_TOME = 7,
        BRACERS = 8,
        LUCKY_GEM = 9,
        JADE_NECKLACE = 10,
        ARTEMIS_BOW = 11,
        WEALTH_BAG = 12,
        THIEF_BELT = 13,
        CRIT_CLOAK = 14,
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Relic")]
    public class RelicSO : ScriptableObject
    {
        public RelicID relic;

        public new string name;

        public string description;

        public Sprite icon;

        [Header("Runtime")]
        public BonusType bonusType;

        public int maxLevel = 1_000;

        public int baseCost;
        public float costPower;

        public float baseEffect;
        public float levelEffect;

        public void Init(RelicStaticData data)
        {
            bonusType = data.bonusType;

            maxLevel = data.maxLevel;

            baseCost = data.baseCost;
            costPower = data.costPower;

            baseEffect = data.baseEffect;
            levelEffect = data.levelEffect;
        }
    }
}