using GM.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public class SquadMercSlot : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text DamageText;

        [SerializeField] private TMP_Text EnergyPercentageText;
        [SerializeField] private Slider EnergySlider;
        [SerializeField] private Slider ExcessEnergySlider;
        [Space]
        public GM.UI.VStackedButton UpgradeButton;
        private int _buyAmount;
        protected int BuyAmount => MathsUtlity.NextMultipleMax(AssignedMerc.CurrentLevel, _buyAmount, AssignedMerc.MaxLevel);

        public void Assign(MercID merc, GM.UI.AmountSelector selector)
        {
            _buyAmount = selector.Current;

            Assign(merc);

            selector.E_OnChange.AddListener((val) => _buyAmount = val);
        }

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;

            UpdateUI();
        }

        private void FixedUpdate()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Energy
            EnergyPercentageText.text = Format.Percentage(AssignedMerc.CurrentSpawnEnergyPercentage, 0);
            EnergySlider.value = Mathf.Clamp(AssignedMerc.CurrentSpawnEnergyPercentage, 0, 1);
            ExcessEnergySlider.value = Mathf.Clamp(AssignedMerc.CurrentSpawnEnergyPercentage - 1, 0, 1);

            LevelText.text = $"Lvl. <color=orange>{AssignedMerc.CurrentLevel}</color>";
            DamageText.text = GetBonusText();

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(AssignedMerc.UpgradeCost(BuyAmount)));
            }

            UpgradeButton.interactable = !AssignedMerc.IsMaxLevel && App.Inventory.Gold >= AssignedMerc.UpgradeCost(BuyAmount);
        }

        private string GetBonusText()
        {
            return $"<color=orange>{Format.Number(AssignedMerc.DamagePerAttack)}</color> DMG";
        }

        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = App.GMCache.MercUpgradeCost(AssignedMerc, BuyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + BuyAmount > AssignedMerc.MaxLevel;
            bool canAffordUpgrade = App.Inventory.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.CurrentLevel += BuyAmount;

                App.Inventory.Gold -= upgradeCost;

                App.Inventory.GoldChanged.Invoke(upgradeCost * -1);
            }
        }

        /// <summary> Callback from UI to open the merc popup </summary>
        public void OnInfoButton()
        {
            this.InstantiateUI<MercPopup>(PopupObject).Assign(AssignedMerc.ID);
        }
    }
}