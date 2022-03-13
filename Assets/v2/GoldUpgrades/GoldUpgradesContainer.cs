using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Common.Enums;
using System;
using System.Linq;

namespace GM.GoldUpgrades
{
    public class GoldUpgradesContainer
    {
        public GoldUpgradeState TapUpgrade = new() { Level = 1 };

        public void DeleteLocalStateData()
        {
            TapUpgrade.Level = 1;
        }
    }
}
