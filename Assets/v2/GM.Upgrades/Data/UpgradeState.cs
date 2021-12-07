using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades.Data
{
    public class UpgradeState
    {
        public int Level = 0;
        public readonly int MaxLevel;

        public IUpgradeUnlockRequire UnlockRequirement { get; private set; }

        public bool IsMaxLevel => Level >= MaxLevel;

        public UpgradeState(int level, int maxLevel)
        {
            Level = level;
            MaxLevel = maxLevel;
            UnlockRequirement = DefaultUpgrade.Value;
        }

        public UpgradeState(int level, int maxLevel, IUpgradeUnlockRequire requirement)
        {
            Level = level;
            MaxLevel = maxLevel;
            UnlockRequirement = requirement;
        }
    }
}