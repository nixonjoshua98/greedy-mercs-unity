using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.PlayerUpgrades.Data
{
    public class PlayerUpgradeState
    {
        public int Level = 0;
        public readonly int MaxLevel;

        public bool IsMaxLevel => Level >= MaxLevel;

        public PlayerUpgradeState(int maxLevel)
        {
            MaxLevel = maxLevel;
        }

        public PlayerUpgradeState(int level, int maxLevel)
        {
            Level = level;
            MaxLevel = maxLevel;
        }
    }
}
