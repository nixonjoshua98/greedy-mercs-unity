
using UnityEngine;

namespace GreedyMercs
{
    public class TapDamageUpgrade : GoldUpgradeRow
    {
        protected override int BuyAmount
        {
            get
            {
                if (PlayerTab.BuyAmount == -1)
                    return Formulas.AffordTapDamageLevels();

                var state = GameState.Upgrades.GetUpgrade(upgrade);

                return Mathf.Min(PlayerTab.BuyAmount, StaticData.MAX_TAP_UPGRADE_LEVEL - state.level);
            }
        }
        void Awake()
        {
            upgrade = GoldUpgradeID.TAP_DAMAGE;
        }

        protected override void UpdateRow()
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

            damageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());

            if (state.level < StaticData.MAX_TAP_UPGRADE_LEVEL)
            {
                string cost = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));

                costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
            }

            else
                costText.text = "MAX";

            nameText.text = string.Format("(Lvl. {0}) Tap Damage", state.level);

            buyButton.interactable = state.level < StaticData.MAX_TAP_UPGRADE_LEVEL;
        }

        // === Button Callbacks ===

        public override void OnBuy()
        {
            int levelsBuying = BuyAmount;

            BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

            UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

            BuyUpgrade(cost, levelsBuying);
        }
    }
}