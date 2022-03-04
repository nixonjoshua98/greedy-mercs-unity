using GM.Upgrades.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public class MajorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.GMCache.MajorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.GMData.Upgrades.MajorTapUpgrade;

        protected override string GetBonusText() => $"<color=orange>{Format.Percentage(Upgrade.Value)}</color> TAP DMG";
    }
}
