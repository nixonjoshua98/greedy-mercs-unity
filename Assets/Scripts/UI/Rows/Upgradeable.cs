
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgradeable : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Text BuyText;
    [SerializeField] protected Text CostText;
    [SerializeField] protected Text LevelText;

    [SerializeField] Button UpgradeButton;

    public virtual bool IsUnlocked { get { return true; } }

    protected int BuyAmount { get { return GetBuyAmount(); } }


    protected abstract int GetBuyAmount();

    public abstract void UpdateRow();

    public abstract void OnBuy();

    protected void UpdateText(UpgradeState state, int maxLevel = int.MaxValue)
    {
        LevelText.text  = "Level " + state.level.ToString();
        BuyText.text    = state.level >= maxLevel ? "" : "x" + BuyAmount.ToString();

        UpgradeButton.interactable = state.level < maxLevel;
    }
}
