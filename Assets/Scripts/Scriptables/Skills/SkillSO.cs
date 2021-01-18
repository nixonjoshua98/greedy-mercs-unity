﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{    
    public enum SkillID
    {
        GOLD_RUSH = 0,
    }

    [System.Serializable]
    public struct SkillLevel
    {
        public double UpgradeCost;
        public double BonusValue;

        public int EnergyCost;
    }


    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Skill")]
    public class SkillSO : ScriptableObject
    {
        public SkillID SkillID;
        public BonusType bonusType;

        public new string name;

        public float Duration;

        public int EnergyGainedOnUnlock;

        public int MaxLevel { get { return Levels.Length; } }

        [SerializeField] SkillLevel[] Levels;

        public SkillLevel GetLevel(int level)
        {
            return Levels[level - 1];
        }
    }
}