using GM.Upgrades.Data;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public class MinorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.GMCache.MinorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.GMData.Upgrades.MinorTapUpgrade;

        protected override string GetBonusText() => $"<color=orange>{Format.Number(App.GMCache.TotalTapDamage)}</color> TAP DMG";
    }
}
