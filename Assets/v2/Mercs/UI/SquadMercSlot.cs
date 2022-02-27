using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnitID = GM.Common.Enums.UnitID;

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
        [Space]
        public GM.UI.VStackedButton UpgradeButton;

        Action<UnitID> RemoveMercFromSquad;

        int _buyAmount;
        protected int BuyAmount => MathUtils.NextMultipleMax(AssignedMerc.CurrentLevel, _buyAmount, Common.Constants.MAX_MERC_LEVEL);

        public void Assign(UnitID merc, GM.UI.AmountSelector selector, Action<UnitID> removeMerc)
        {
            _buyAmount = selector.Current;
            RemoveMercFromSquad = removeMerc;

            Assign(merc); 

            selector.E_OnChange.AddListener((val) => _buyAmount = val);
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


        public void OnUpgradeButton()
        {
            BigDouble upgradeCost = App.Cache.MercUpgradeCost(AssignedMerc, BuyAmount);

            bool willExceedMaxLevel = AssignedMerc.CurrentLevel + BuyAmount > Common.Constants.MAX_MERC_LEVEL;
            bool canAffordUpgrade = App.Data.Inv.Gold >= upgradeCost;

            if (!willExceedMaxLevel && canAffordUpgrade)
            {
                AssignedMerc.CurrentLevel += BuyAmount;

                App.Data.Inv.Gold -= upgradeCost;

                App.Events.GoldChanged.Invoke(upgradeCost * -1);
            }
        }

        public void OnRemoveSquadMercButton()
        {
            RemoveMercFromSquad.Invoke(AssignedMerc.Id);
        }

        /// <summary> Callback from UI to open the merc popup </summary>
        public void OnInfoButton()
        {
            InstantiateUI<MercPopup>(PopupObject).Assign(AssignedMerc.Id);
        }
    }
}