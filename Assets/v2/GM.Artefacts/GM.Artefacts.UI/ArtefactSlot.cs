using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AmountSelector = GM.UI_.AmountSelector;
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
        public UI_.VStackedButton UpgradeButton;

        int _BuyAmount; // Raw value. We should use BuyAmount for most upgrading cases

        int BuyAmount => MathUtils.NextMultipleMax(AssignedArtefact.CurrentLevel, _BuyAmount, AssignedArtefact.MaxLevel);

        public void AssignArtefact(int artefactId, AmountSelector selector)
        {
            base.AssignArtefact(artefactId);

            // Set the callback for when the user changes the buy amount
            selector.E_OnChange.AddListener(val => {
                _BuyAmount = val;

                SetUpgradeRelatedText();
            });

            // Assign a default buy amount
            _BuyAmount = selector.Current;

            // Set some static values
            NameText.text = AssignedArtefact.Name;
            IconImage.sprite = AssignedArtefact.Icon;

            // Update the level related values to default
            SetUpgradeRelatedText();
        }

        void SetUpgradeRelatedText()
        {
            UpgradeButton.SetText("Max Level", "-");

            if (!AssignedArtefact.IsMaxLevel)
            {
                BigInteger ugradeCost = App.Cache.ArtefactUpgradeCost(AssignedArtefact, BuyAmount);

                UpgradeButton.SetText($"x{BuyAmount}", FormatString.Number(ugradeCost));
            }

            LevelText.text = $"Level {AssignedArtefact.CurrentLevel}";
            BonusText.text = FormatString.Bonus(AssignedArtefact.Bonus, AssignedArtefact.BaseEffect);

            UpgradeButton.interactable = !AssignedArtefact.IsMaxLevel;
        }


        // == Callbacks == //
        public void OnUpgradeButton()
        {
            App.Data.Arts.UpgradeArtefact(AssignedArtefactId, BuyAmount, (success) =>
            {
                SetUpgradeRelatedText();
            });
        }

        public void OnShowPopupButton()
        {
            CanvasUtils.Instantiate<ArtefactPopup>(ArtefactPopupObject)
                .AssignArtefact(AssignedArtefactId);
        }
    }
}