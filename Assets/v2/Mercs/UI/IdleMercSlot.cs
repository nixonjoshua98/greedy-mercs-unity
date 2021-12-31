using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MercID = GM.Common.Enums.MercID;
using System;
using AmountSelector = GM.UI.AmountSelector;
using VStackedButton = GM.UI.VStackedButton;

namespace GM.Mercs.UI
{
    public class IdleMercSlot : MercUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public Image IconImage;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text DamageText;
        [Space]
        public VStackedButton UpgradeButton;

        // Callbacks
        Action<MercID> AddToSquad;

        // Buy amount calculations
        int _buyAmount;
        protected int BuyAmount => MathUtils.NextMultipleMax(AssignedMerc.CurrentLevel, _buyAmount, Common.Constants.MAX_MERC_LEVEL);

        public void Assign(MercID mercId, AmountSelector selector, Action<MercID> addToSquad)
        {
            _buyAmount = selector.Current;
            AddToSquad = addToSquad;

            Assign(mercId);

            selector.E_OnChange.AddListener((val) => { _buyAmount = val; });
        }

        protected override void OnAssigned()
        {
            IconImage.sprite = AssignedMerc.Icon;
            NameText.text = AssignedMerc.Name;

            UpdateUI();
        }

        void FixedUpdate()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            LevelText.text = FormatLevel(AssignedMerc.CurrentLevel);
            DamageText.text = GetBonusText();

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedMerc.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(AssignedMerc.UpgradeCost(BuyAmount)));
            }

            UpgradeButton.interactable = !AssignedMerc.IsMaxLevel && App.Data.Inv.Gold >= AssignedMerc.UpgradeCost(BuyAmount);
        }

        string GetBonusText() => $"<color=orange>{Format.Number(AssignedMerc.DamagePerAttack)}</color> DMG";

        public void OnSquadAddButton()
        {
            AddToSquad(AssignedMerc.Id);
        }

        public void OnInfoButton()
        {
            InstantiateUI<MercPopup>(PopupObject).Assign(AssignedMerc.Id);
        }

        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = AssignedMerc.UpgradeCost(BuyAmount);

            bool willExceedMaxLevel = (AssignedMerc.CurrentLevel + BuyAmount) > AssignedMerc.MaxLevel;
            bool canAffordUpgrade = App.Data.Inv.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                App.Data.Inv.Gold -= upgradeCost;

                AssignedMerc.CurrentLevel += BuyAmount;
                
                App.Events.GoldChanged.Invoke(upgradeCost * -1);
            }
        }
    }
}