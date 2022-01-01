using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using System;
using System.Linq;

namespace GM.Upgrades.Data
{
    public class PlayerUpgrades
    {
        public UpgradeState MinorTapUpgrade { get; private set; } = new UpgradeState(1, 1_000, BonusType.FLAT_TAP_DMG);
        public UpgradeState MajorTapUpgrade { get; private set; } = new UpgradeState(0, 1_000, BonusType.MULTIPLY_TAP_DMG, new UpgradeGoldRequirement(BigDouble.Billion));

        public Dictionary<UpgradeID, UpgradeState> Upgrades = new Dictionary<UpgradeID, UpgradeState>();

        public PlayerUpgrades()
        {
            Upgrades[UpgradeID.FLAT_TAP_DMG] = MinorTapUpgrade;
            Upgrades[UpgradeID.MULT_TAP_DMG] = MajorTapUpgrade;

            Upgrades.Keys.ToList().ForEach(key => { Upgrades[key].Id = key; });
        }

        public void ResetLevels()
        {
            foreach (UpgradeState state in Upgrades.Values)
            {
                state.Level = 0;
            }
        }
    }
}
