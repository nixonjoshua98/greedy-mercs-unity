
using UnityEngine;
using UnityEngine.UI;

public class TapDamageUpgrade : MonoBehaviour
{
    [SerializeField] UpgradeID upgrade;

    [Header("Components")]
    [SerializeField] Button buyButton;
    [Space]
    [SerializeField] Text costText;
    [SerializeField] Text levelText;
    [SerializeField] Text damageText;

    int BuyAmount
    {
        get
        {
            if (PlayerTab.BuyAmount == -1)
                return Formulas.AffordTapDamageLevels();

            var state = GameState.Upgrades.GetUpgrade(upgrade);

            return Mathf.Min(PlayerTab.BuyAmount, StaticData.MAX_TAP_UPGRADE_LEVEL - state.level);
        }
    }

    void FixedUpdate()
    {
        UpdateRow();
    }

    public void UpdateRow()
    {
        UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

        damageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());
        levelText.text  = "Level " + state.level.ToString();

        if (state.level < StaticData.MAX_CHAR_LEVEL)
        {
            string cost = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));

            costText.text = string.Format("x{0}\n{1}", BuyAmount, cost);
        }

        else
            costText.text = "MAX";

        buyButton.interactable = state.level < StaticData.MAX_TAP_UPGRADE_LEVEL;
    }

    // === Button Callbacks ===

    public void OnBuy()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(upgrade);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;
        }
    }
}