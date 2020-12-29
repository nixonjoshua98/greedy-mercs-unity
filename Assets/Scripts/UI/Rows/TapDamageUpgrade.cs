
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

        return PlayerTab.BuyAmount;
    }

    public override void UpdateRow()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(upgradeId);

        DamageText.text = Utils.Format.FormatNumber(StatsCache.GetTapDamage());
        CostText.text   = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));

        UpdateText(state);
    }

    // === Button Callbacks ===

    public override void OnBuy()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(upgradeId);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }
}
