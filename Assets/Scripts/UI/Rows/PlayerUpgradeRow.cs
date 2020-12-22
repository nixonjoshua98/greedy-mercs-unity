
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeRow : MonoBehaviour
{
    [SerializeField] PlayerUpgradeID playerUpgrade;
    [Space]
    [SerializeField] Text DamageText;
    [SerializeField] Text BuyText;
    [SerializeField] Text CostText;
    [SerializeField] Text LevelText;

    void OnEnable()
    {
        InvokeRepeating("UpdateRow", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        if (IsInvoking("UpdateRow"))
        {
            CancelInvoke("UpdateRow");
        }
    }

    void UpdateRow()
    {
        PlayerUpgradeState state = GameState.player.GetUpgradeState(playerUpgrade);

        LevelText.text      = "Level " + state.level.ToString();
        BuyText.text        = "x" + PlayerTab.BuyAmount.ToString();
        DamageText.text     = Utils.Format.DoubleToString(StatsCache.GetTapDamage());
        CostText.text       = Utils.Format.DoubleToString(Formulas.CalcTapDamageLevelUpCost(PlayerTab.BuyAmount));
    }

    // === Button Callbacks ===

    public void OnBuyButton()
    {
        int levelsBuying = PlayerTab.BuyAmount;

        double cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

        if (GameState.player.gold >= cost)
        {
            var state = GameState.player.GetUpgradeState(playerUpgrade);

            state.level += levelsBuying;

            GameState.player.gold -= cost;

            UpdateRow();
        }
    }
}
