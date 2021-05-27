
using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    using GM.UI;

    public class TapDamageUpgrade : MonoBehaviour
    {
        GoldUpgradeID upgrade = GoldUpgradeID.TAP_DAMAGE;

        UpgradeState State { get { return GameState.Upgrades.GetUpgrade(upgrade); } }

        [SerializeField] BuyController controller;

        [Header("Components")]
        [SerializeField] StackedButton upgradeButton;
        [Space]
        [SerializeField] Button buyButton;
        [Space]
        [SerializeField] Text nameText;
        [SerializeField] Text damageText;

        int _buyAmount;

        int buyAmount
        {
            get
            {
                if (_buyAmount == -1)
                {
                    return Formulas.AffordTapDamageLevels();
                }

                return Mathf.Min(_buyAmount, StaticData.MAX_TAP_UPGRADE_LEVEL - State.level);
            }
        }


        void Start()
        {
            controller.AddListener((amount) => { _buyAmount = amount; });
        }

        void FixedUpdate()
        {
            UpdateButtonText();

            damageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());

            nameText.text = string.Format("(Lvl. {0}) Tap Damage", State.level);

            buyButton.interactable = State.level < StaticData.MAX_TAP_UPGRADE_LEVEL;
        }

        void UpdateButtonText()
        {
            string top = "MAX";
            string btm = "-";

            if (State.level < StaticData.MAX_TAP_UPGRADE_LEVEL)
            {
                top = string.Format("x{0}", buyAmount);
                btm = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(buyAmount));
            }

            upgradeButton.SetText(top, btm);
        }

        // === Button Callbacks ===

        public void OnBuy()
        {
            int levelsBuying = buyAmount;

            BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

            if (GameState.Player.gold >= cost)
            {
                UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

                state.level += levelsBuying;

                GameState.Player.gold -= cost;
            }
        }
    }
}