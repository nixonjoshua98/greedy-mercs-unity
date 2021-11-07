using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AmountSelector = GM.UI.AmountSelector;
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
        public GM.UI.VStackedButton UpgradeButton;

        int _BuyAmount; // Raw value. We should use BuyAmount for most cases

        int BuyAmount => MathUtils.NextMultipleMax(AssignedArtefact.CurrentLevel, _BuyAmount, AssignedArtefact.MaxLevel);

        public void AssignArtefact(int artefactId, AmountSelector selector)
        {
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

            LevelText.text = $"Lvl. <color=orange>{AssignedArtefact.CurrentLevel}</color>";
            BonusText.text = Format.Bonus(AssignedArtefact.Bonus, AssignedArtefact.BaseEffect);

            UpgradeButton.interactable = !AssignedArtefact.IsMaxLevel && ugradeCost < App.Data.Inv.PrestigePoints;
        }


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            App.Data.Arts.UpgradeArtefact(AssignedArtefact.Id, BuyAmount, (success) =>
            {
                UpdateUI();
            });

            UpdateUI(); // We have this here to force an update with the new dummy level
        }

        public void OnShowPopupButton()
        {
            CanvasUtils.Instantiate<ArtefactPopup>(ArtefactPopupObject).AssignArtefact(AssignedArtefact.Id);
        }
    }
}