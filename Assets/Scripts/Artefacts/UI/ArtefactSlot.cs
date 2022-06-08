using GM.Common;
using GM.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactSlot : ArtefactUIObject
    {
        [Header("Prefabs")]
        public GameObject ArtefactPopupObject;

        [Header("References")]
        public Image IconImage;
        public Image IconBackgroundImage;
        [Space]
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text BonusText;
        [Space]
        public VStackedButton UpgradeButton;
        private Action<int, int> upgradeCallback;
        private int _BuyAmount; // Raw value. We should use BuyAmount for most cases

        private int BuyAmount => MathsUtlity.NextMultipleMax(AssignedArtefact.CurrentLevel, _BuyAmount, AssignedArtefact.MaxLevel);

        public void Setup(int artefactId, AmountSelector selector, Action<int, int> callback)
        {
            upgradeCallback = callback;

            // Set the callback for when the user changes the buy amount
            selector.E_OnChange.AddListener(val =>
            {
                _BuyAmount = val;

                UpdateUI();
            });

            // Assign a default buy amount
            _BuyAmount = selector.Current;

            base.AssignArtefact(artefactId);
        }

        protected override void OnAssigned()
        {
            SetStaticUI();
            UpdateUI();
        }

        private void SetStaticUI()
        {
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;

            if (AssignedArtefact.IconBackground != null) IconBackgroundImage.sprite = AssignedArtefact.IconBackground;
        }

        private void UpdateUI()
        {
            double ugradeCost = AssignedArtefact.UpgradeCost(BuyAmount);

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedArtefact.IsMaxLevel)
            {
                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(ugradeCost));
            }

            LevelText.text = $"Lvl. <color=orange>{AssignedArtefact.CurrentLevel}</color>";
            BonusText.text = GetBonusText();

            UpgradeButton.interactable = !AssignedArtefact.IsMaxLevel && ugradeCost < App.Inventory.PrestigePoints;
        }




        // == Callbacks == //
        public void OnUpgradeButton()
        {
            upgradeCallback.Invoke(AssignedArtefact.Id, BuyAmount);

            UpdateUI();
        }

        public void OnShowPopupButton()
        {
            this.InstantiateUI<ArtefactPopup>(ArtefactPopupObject).AssignArtefact(AssignedArtefact.Id);
        }
    }
}