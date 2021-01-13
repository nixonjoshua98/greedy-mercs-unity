using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SkillData
{    public enum SkillID
    {
        GOLD_RUSH = 0,
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Skill")]
    public class SkillSO : ScriptableObject
    {
        public SkillID SkillID;

        public float Duration;

        public double UnlockCost;

        public int EnergyCost;

        public int EnergyGainedOnUnlock;

        [Header("Bonus")]
        public BonusType bonusType;
        public double bonusValue;
    }
}