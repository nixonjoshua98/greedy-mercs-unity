
using UnityEngine;
using UnityEngine.UI;

public class TapDamageUpgrade : MonoBehaviour
{
    [SerializeField] UpgradeID upgrade;

    [Header("Components")]
    [SerializeField] Button buyButton;
    [Space]
    [SerializeField] Text buyText;
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
        costText.text   = state.level >= StaticData.MAX_TAP_UPGRADE_LEVEL ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));
        levelText.text  = "Level " + state.level.ToString();
        buyText.text    = state.level >= StaticData.MAX_TAP_UPGRADE_LEVEL ? "" : "x" + BuyAmount.ToString();

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