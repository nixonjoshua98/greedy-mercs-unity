using SRC.Common;
using TMPro;
using UnityEngine;

namespace SRC.GoldUpgrades.UI
{
    public class TapDamageGoldUpgradeSlot : SRC.Core.GMMonoBehaviour
    {
        [Header("Components")]
        public TMP_Text LevelText;
        public TMP_Text DamageText;
        [Space]
        public SRC.UI.StackedButton UpgradeButton;
        private int _buyAmount;
        protected int BuyAmount => MathsUtlity.NextMultipleMax(Upgrade.Level, _buyAmount, 1_000);

        private TapDamageGoldUpgrade Upgrade => App.GoldUpgrades.TapUpgrade;

        private void FixedUpdate()
        {
            UpdateButton();

            LevelText.text = $"Lvl. <color=orange>{Upgrade.Level}</color>";
            DamageText.text = $"<color=orange>{Format.Number(App.Values.TotalTapDamage)}</color> DMG";
        }

        private void UpdateButton()
        {
            UpgradeButton.SetText("MAX LEVEL", "");

            if (!Upgrade.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(Upgrade.UpgradeCost(BuyAmount)));
            }

            UpgradeButton.interactable = !Upgrade.IsMaxLevel && App.Inventory.Gold >= Upgrade.UpgradeCost(BuyAmount);
        }

        // = Callbacks = //

        public void AmountSelector_ValueChanged(int newValue)
        {
            _buyAmount = newValue;
        }

        public void Button_Upgrade()
        {
            BigDouble upgradeCost = Upgrade.UpgradeCost(BuyAmount);

            bool canAffordUpgrade = App.Inventory.Gold >= upgradeCost;

            if (canAffordUpgrade)
            {
                Upgrade.Level += BuyAmount;

                App.Inventory.Gold -= upgradeCost;
            }
        }
    }
}
