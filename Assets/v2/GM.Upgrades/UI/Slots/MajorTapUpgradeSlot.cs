using GM.Upgrades.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public class MajorTapUpgradeSlot : UpgradeSlot<UpgradeState>
    {
        protected override BigDouble UpgradeCost => App.Cache.MajorTapUpgradeCost(BuyAmount);
        protected override UpgradeState Upgrade => App.Data.Upgrades.MajorTapUpgrade;

        BigDouble DamageFromUpgrade => App.Cache.MajorTapUpgradeDamage;

        protected override string GetBonusText() => $"<color=orange>{Format.Number(DamageFromUpgrade)}x</color> TAP DMG";
        public override void OnUnlocked()
        {
            gameObject.SetActive(true);
        }
    }
}
