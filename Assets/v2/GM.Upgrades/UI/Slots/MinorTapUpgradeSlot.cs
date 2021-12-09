using GM.Upgrades.Data;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public class MinorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.Cache.MinorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.Data.Upgrades.MinorTapUpgrade;

        protected override string GetBonusText() => $"<color=orange>{Format.Number(App.Cache.TotalTapDamage)}</color> TAP DMG";
    }
}
