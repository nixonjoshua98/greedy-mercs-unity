using GM.UI;
using GM.Upgrades.Data;
using TMPro;
using UnityEngine;

namespace GM.Upgrades.UI
{
    public interface IUpgradeSlot
    {
        void Init(AmountSelector selector);
        void OnUnlocked();
        void Hide();
    }

    public abstract class UpgradeSlot<T> : SlotObject, IUpgradeSlot where T: UpgradeState
    {
        [Header("Components")]
        public TMP_Text LevelText;
        public TMP_Text BonusText;
        public VStackedButton UpgradeButton;

        protected int _buyAmount;

        protected abstract BigDouble UpgradeCost { get; }
        protected abstract T Upgrade { get; }
        protected int BuyAmount => MathUtils.NextMultipleMax(Upgrade.Level, _buyAmount, Upgrade.MaxLevel);

        public virtual void Init(AmountSelector selector)
        {
            _buyAmount = selector.Current;

            selector.E_OnChange.AddListener((val) => { _buyAmount = val; });

            App.Data.Inv.E_GoldChange.AddListener(_ => UpdateUI());
        }

        /// <summary>
        /// Useful when referencing the slot usin the interface
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        void FixedUpdate()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            UpgradeButton.SetText("MAX LEVEL", "");

            if (!Upgrade.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{Format.Number(BuyAmount)}", Format.Number(UpgradeCost));
            }

            LevelText.text = FormatLevel(Upgrade.Level);
            BonusText.text = GetBonusText();

            UpgradeButton.interactable = !Upgrade.IsMaxLevel && App.Data.Inv.Gold >= UpgradeCost;
        }

        protected abstract string GetBonusText();
        public abstract void OnUnlocked();

        public void OnUpgradeButton()
        {
            if (App.Data.Inv.Gold >= UpgradeCost)
            {
                App.Data.Inv.Gold -= UpgradeCost;

                Upgrade.Level += BuyAmount;

                App.Data.Inv.E_GoldChange.Invoke(UpgradeCost * -1);
            }
        }
    }
}
