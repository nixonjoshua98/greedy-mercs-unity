using GM.Upgrades.Data;

namespace GM.Upgrades.UI
{
    public class MinorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.Cache.MinorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.Data.Upgrades.MinorTapUpgrade;

        BigDouble TotalTapDamage => App.Cache.TotalTapDamage;

        protected override string GetBonusText() => $"<color=orange>{Format.Number(TotalTapDamage)}</color> TAP DMG";
    }
}
