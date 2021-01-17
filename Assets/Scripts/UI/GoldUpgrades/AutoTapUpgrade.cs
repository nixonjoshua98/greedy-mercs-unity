
using UnityEngine;


namespace UI.GoldUpgrades
{
    public class AutoTapUpgrade : GoldUpgradeRow
    {
        protected override int BuyAmount
        {
            get
            {
                if (PlayerTab.BuyAmount == -1)
                    return Formulas.GoldUpgrades.AffordTapDamageLevels();

                var state = GameState.Upgrades.GetUpgrade(upgrade);

                return Mathf.Min(PlayerTab.BuyAmount, StaticData.MAX_AUTO_TAP_LEVEL - state.level);
            }
        }

        void Awake()
        {
            upgrade = GoldUpgradeID.AUTO_TAP_DMG;
        }

        public override void OnBuy()
        {
            int levelsBuying = BuyAmount;

            BigDouble cost = Formulas.GoldUpgrades.CalcAutoTapsLevelUpCost(levelsBuying);

            BuyUpgrade(cost, levelsBuying);
        }

        protected override void UpdateRow()
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

            damageText.text = Utils.Format.FormatNumber(StatsCache.GoldUpgrades.AutoTapDamage()) + " DPS";

            if (state.level < StaticData.MAX_AUTO_TAP_LEVEL)
            {
                string cost = Utils.Format.FormatNumber(Formulas.GoldUpgrades.CalcAutoTapsLevelUpCost(BuyAmount));

                costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
            }

            else
                costText.text = "MAX";

            nameText.text = string.Format("(Lvl. {0}) Auto Clicker", state.level);

            buyButton.interactable = state.level < StaticData.MAX_AUTO_TAP_LEVEL;
        }
    }
}