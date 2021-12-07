using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;

namespace GM.Upgrades.Data
{
    public class PlayerUpgrades
    {
        public UpgradeState MinorTapUpgrade { get; private set; } = new UpgradeState(1, 1_000);
        public UpgradeState MajorTapUpgrade { get; private set; } = new UpgradeState(0, 1_000);

    }
}
