using GM.UI;
using System;
using System.Numerics;
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

        Action<int, int> upgradeCallback;
        int _BuyAmount; // Raw value. We should use BuyAmount for most cases

        int BuyAmount => MathUtils.NextMultipleMax(AssignedArtefact.CurrentLevel, _BuyAmount, AssignedArtefact.MaxLevel);

        public void Setup(int artefactId, AmountSelector selector, Action<int, int> callback)
        {
            upgradeCallback = callback;

            // Set the callback for when the user changes the buy amount
            selector.E_OnChange.AddListener(val => {
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

        void SetStaticUI()
        {
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;

            if (AssignedArtefact.IconBackground != null) IconBackgroundImage.sprite = AssignedArtefact.IconBackground;
        }

        void UpdateUI()
        {
            BigInteger ugradeCost;

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedArtefact.IsMaxLevel)
            {
                ugradeCost = App.GMCache.ArtefactUpgradeCost(AssignedArtefact, BuyAmount);

                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(ugradeCost));
            }

            LevelText.text = FormatLevel(AssignedArtefact.CurrentLevel);
            BonusText.text = GetBonusText();

            UpgradeButton.interactable = !AssignedArtefact.IsMaxLevel && ugradeCost < App.DataContainers.Inv.PrestigePoints;
        }


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            upgradeCallback.Invoke(AssignedArtefact.Id, BuyAmount);

            UpdateUI();
        }

        public void OnShowPopupButton()
        {
            InstantiateUI<ArtefactPopup>(ArtefactPopupObject).AssignArtefact(AssignedArtefact.Id);
        }
    }
}