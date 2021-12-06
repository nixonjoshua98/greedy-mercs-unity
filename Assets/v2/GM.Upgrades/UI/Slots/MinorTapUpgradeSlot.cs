using GM.Upgrades.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public class MinorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.Cache.MinorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.Data.Upgrades.MinorTapUpgrade;

        BigDouble DamageFromUpgrade => App.Cache.MinorTapUpgradeDamage;

        protected override string GetBonusText() => $"<color=orange>{Format.Number(DamageFromUpgrade)}</color> DMG";
    }
}
