
using UnityEngine;
using UnityEngine.UI;

public class TapDamageUpgrade : UpgradeRow
{
    [SerializeField] protected Text DamageText;

    [Space]

    [SerializeField] UpgradeID upgradeId;

    protected override int GetBuyAmount()
    {
        if (PlayerTab.BuyAmount == -1)
            return Formulas.AffordTapDamageLevels();

        var state = GameState.Upgrades.GetUpgrade(upgradeId);

        return Mathf.Min(PlayerTab.BuyAmount, StaticData.MAX_TAP_UPGRADE_LEVEL - state.level);
    }

    public override void UpdateRow()
    {
        UpgradeState state = GameState.Upgrades.GetUpgrade(upgradeId);

        DamageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());
        CostText.text   = state.level >= StaticData.MAX_TAP_UPGRADE_LEVEL ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));

        UpdateText(state, StaticData.MAX_TAP_UPGRADE_LEVEL);
    }

    // === Button Callbacks ===

    public override void OnBuy()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(upgradeId);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }
}