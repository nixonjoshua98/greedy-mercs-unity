using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using System;

namespace GM.Upgrades.Data
{
    public class Upgrades
    {
        public UpgradeState MinorTapUpgrade { get; private set; } = new UpgradeState(1, 1_000);
        public UpgradeState MajorTapUpgrade { get; private set; } = new UpgradeState(0, 1_000, new UpgradeGoldRequirement(BigDouble.Billion));

        public UpgradeState GetUpgrade(UpgradeID upgrade)
        {
            return upgrade switch
            {
                UpgradeID.MINOR_TAP => MinorTapUpgrade,
                UpgradeID.MAJOR_TAP => MajorTapUpgrade,
                _ => throw new Exception($"Upgrade '{upgrade}' not found")
            };
        }
    }
}
