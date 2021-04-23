
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
            UpdateButtonText();

            damageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());

            nameText.text = string.Format("(Lvl. {0}) Tap Damage", State.level);

            buyButton.interactable = State.level < StaticData.MAX_TAP_UPGRADE_LEVEL;
        }

        void UpdateButtonText()
        {
            costText.text       = "-";
            purchaseText.text   = "MAX LEVEL";

            if (State.level < StaticData.MAX_TAP_UPGRADE_LEVEL)
            {
                purchaseText.text = string.Format("x{0}", BuyAmount);

                costText.text = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));
            }
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