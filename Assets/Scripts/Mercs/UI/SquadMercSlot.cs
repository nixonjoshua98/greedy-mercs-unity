using SRC.Common;
using SRC.Mercs.Data;
using SRC.UI;
using TMPro;
using UnityEngine;

namespace SRC.Mercs.UI
{
    public class SquadMercSlot : MercUIObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject PopupObject;
        [SerializeField] private GameObject PassiveIcon;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text LevelText;
        [SerializeField] private TMP_Text DamageText;

        [Space]

        [SerializeField] private Transform PassiveIconsParent;
        [SerializeField] private GenericGradeItem GradeSlot;
        [SerializeField] private StackedButton UpgradeButton;

        private int _buyAmount;
        protected int BuyAmount => MathsUtlity.NextMultipleMax(AssignedMerc.CurrentLevel, _buyAmount, AssignedMerc.MaxLevel);

        public void Assign(MercID merc, SRC.UI.IntegerSelector selector)
        {
            _buyAmount = selector.CurrentValue;

            Assign(merc);

            selector.E_OnChange.AddListener((val) =>
            {
                _buyAmount = val;

                UpdateUI();
            });

            InvokeRepeating(nameof(UpdateUI), 0f, 0.25f);
        }

        protected override void OnAssigned()
        {
            InstantiatePassiveIcons();

            GradeSlot.Intialize(AssignedMerc);

            NameText.text = AssignedMerc.Name;

            UpdateUI();
        }

        private void InstantiatePassiveIcons()
        {
            foreach (var passive in AssignedMerc.Passives)
            {
                var slot = this.Instantiate<MercSlotPassiveIcon>(PassiveIcon, PassiveIconsParent);

                slot.Initialize(AssignedMerc, passive.PassiveID);
            }
        }

        private void UpdateUI()
        {
            UpdateUpgradeButton();

            LevelText.text = $"Lv <color=orange>{AssignedMerc.CurrentLevel}</color>";
            DamageText.text = $"<color=orange>{Format.Number(AssignedMerc.DamagePerAttack)}</color> DMG";
        }

        private void UpdateUpgradeButton()
        {
            UpgradeButton.Icon.enabled = !AssignedMerc.IsMaxLevel;

            UpgradeButton.TopText.text = "Max Level";
            UpgradeButton.BtmText.text = "";

            BigDouble upgradeCost = AssignedMerc.UpgradeCost(BuyAmount);

            if (!AssignedMerc.IsMaxLevel)
            {
                UpgradeButton.TopText.text = $"x{BuyAmount}";
                UpgradeButton.BtmText.text = Format.Number(upgradeCost);
            }

            UpgradeButton.interactable = !AssignedMerc.IsMaxLevel && App.Inventory.Gold >= upgradeCost;
        }

        /* Event Listeners */

        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = App.Values.MercUpgradeCost(AssignedMerc, BuyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + BuyAmount > AssignedMerc.MaxLevel;
            bool canAffordUpgrade = App.Inventory.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.CurrentLevel += BuyAmount;

                App.Inventory.Gold -= upgradeCost;

                App.Inventory.GoldChanged.Invoke(upgradeCost * -1);
            }

            UpdateUI();
        }

        public void OnInfoButton()
        {
            var popup = this.InstantiateUI<MercPopup>(PopupObject);

            popup.Initialize(AssignedMerc);
        }
    }
}