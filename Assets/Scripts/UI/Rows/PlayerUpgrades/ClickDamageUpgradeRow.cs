
using UnityEngine;
using UnityEngine.UI;

public class ClickDamageUpgradeRow : MonoBehaviour
{
    [SerializeField] UpgradeID playerUpgrade;
    [Space]
    [SerializeField] Text DamageText;
    [SerializeField] Text BuyText;
    [SerializeField] Text CostText;
    [SerializeField] Text LevelText;

    int BuyAmount
    {
        get
        {
            if (PlayerTab.BuyAmount == -1)
                return Mathf.Max(1, Formulas.CalcAffordableTapDamageLevels());

            return PlayerTab.BuyAmount;
        }
    }

    void Awake()
    {
        if (!GameState.PlayerUpgrades.TryGetUpgrade(playerUpgrade, out UpgradeState _))
            GameState.PlayerUpgrades.AddUpgrade(playerUpgrade);
    }

    void OnEnable()
    {
        InvokeRepeating("UpdateRow", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        if (IsInvoking("UpdateRow"))
            CancelInvoke("UpdateRow");
    }

    void UpdateRow()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(playerUpgrade);

        LevelText.text      = "Level " + state.level.ToString();
        BuyText.text        = "x" + BuyAmount.ToString();
        DamageText.text     = Utils.Format.FormatNumber(StatsCache.GetTapDamage());
        CostText.text       = Utils.Format.FormatNumber(Formulas.CalcTapDamageLevelUpCost(BuyAmount));
    }

    // === Button Callbacks ===

    public void OnBuyButton()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcTapDamageLevelUpCost(levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(playerUpgrade);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }
}
