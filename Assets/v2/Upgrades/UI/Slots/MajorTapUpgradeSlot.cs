using GM.Upgrades.Data;

namespace GM.Upgrades.UI
{
    public class MajorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.GMCache.MajorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.DataContainers.Upgrades.MajorTapUpgrade;

        protected override string GetBonusText() => $"<color=orange>{Format.Percentage(Upgrade.Value.ToDouble())}</color> TAP DMG";
    }
}
