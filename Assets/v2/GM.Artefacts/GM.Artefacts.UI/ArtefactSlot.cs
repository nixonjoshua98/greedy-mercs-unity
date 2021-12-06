using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GM.UI;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Artefacts.UI
{
    public class ArtefactSlot : ArtefactUIObject
    {
        [Header("Prefabs")]
        public GameObject ArtefactPopupObject;

        [Header("References")]
        public Image IconImage;
        [Space]
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text BonusText;
        [Space]
        public VStackedButton UpgradeButton;

        AmountSelector amountSelector;
        int _BuyAmount; // Raw value. We should use BuyAmount for most cases

        int BuyAmount => MathUtils.NextMultipleMax(AssignedArtefact.CurrentLevel, _BuyAmount, AssignedArtefact.MaxLevel);

        public void AssignArtefact(int artefactId, AmountSelector selector)
        {
            amountSelector = selector;

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
            // Set some static values
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;

            // Update the level related values to default
            UpdateUI();
        }

        void UpdateUI()
        {
            BigInteger ugradeCost;

            UpgradeButton.SetText("MAX LEVEL", "");

            if (!AssignedArtefact.IsMaxLevel)
            {
                ugradeCost = App.Cache.ArtefactUpgradeCost(AssignedArtefact, BuyAmount);

                UpgradeButton.SetText($"x{BuyAmount}", Format.Number(ugradeCost));
            }

            LevelText.text = FormatLevel(AssignedArtefact.CurrentLevel);
            BonusText.text = GetBonusText();

            UpgradeButton.interactable = !AssignedArtefact.IsMaxLevel && ugradeCost < App.Data.Inv.PrestigePoints;
        }

        string GetBonusText() => $"<color=orange>{Format.Number(AssignedArtefact.BaseEffect)}</color> {Format.Bonus(AssignedArtefact.Bonus)}";


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            App.Data.Artefacts.UpgradeArtefact(AssignedArtefact.Id, BuyAmount, (success) =>
            {
                UpdateUI();

                amountSelector.ReInvoke(); // Force a UI update for the other artefacts
            });
        }

        public void OnShowPopupButton()
        {
            InstantiateUI<ArtefactPopup>(ArtefactPopupObject).AssignArtefact(AssignedArtefact.Id);
        }
    }
}