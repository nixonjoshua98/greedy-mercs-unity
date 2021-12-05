using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BonusType = GM.Common.Enums.BonusType;
using GM.PlayerUpgrades.Data;
using GM.UI;

namespace GM.PlayerUpgrades.UI
{
    public class TapDamageUpgradeSlot : PlayerUpgradeUIObject
    {
        protected override UpgradeID UpgradeId { get; set; } = UpgradeID.MINOR_TAP_DAMAGE;
        protected BigDouble UpgradeCost => App.Cache.MinorTapUpgradeCost(BuyAmount);
        protected BigDouble DamageFromUpgrade => App.Cache.MinorTapUpgradeDamage;


        [Header("Components")]
        public TMP_Text LevelText;
        public TMP_Text DamageText;
        public VStackedButton UpgradeButton;

        int _buyAmount;
        protected int BuyAmount => MathUtils.NextMultipleMax(Upgrade.Level, _buyAmount, Upgrade.MaxLevel);

        public override void Init(AmountSelector selector)
        {
            _buyAmount = selector.Current;

            selector.E_OnChange.AddListener((val) => { _buyAmount = val; });
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
            DamageText.text = $"<color=orange>{Format.Number(DamageFromUpgrade)}</color> {Format.Bonus(BonusType.FLAT_TAP_DAMAGE)}";

            UpgradeButton.interactable = !Upgrade.IsMaxLevel && App.Data.Inv.Gold >= UpgradeCost;
        }

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
