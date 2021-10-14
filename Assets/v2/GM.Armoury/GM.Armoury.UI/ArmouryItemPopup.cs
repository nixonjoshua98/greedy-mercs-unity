using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI_
{
    public class ArmouryItemPopup : ArmouryItemUIObject
    {
        [Header("References")]
        public Button EvolveButton;
        [Space]
        public TMP_Text TierText;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text OwnedText;
        [Space]
        public Image IconImage;
        public Slider EvolveProgressSlider;

        public override void AssignItem(int itemId)
        {
            base.AssignItem(itemId);
            SetStaticUIElements();
            RefreshUI();
        }

        void SetStaticUIElements()
        {
            TierText.color = AssignedItem.DisplayConfig.Colour;
            TierText.text = AssignedItem.DisplayConfig.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void RefreshUI()
        {
            LevelText.text = $"Level {AssignedItem.CurrentLevel}";
            EvolveButton.interactable = AssignedItem.CanStarUpgrade;
            OwnedText.text = $"{AssignedItem.NumOwned} / {AssignedItem.StarLevelCost}";
            EvolveProgressSlider.value = AssignedItem.NumOwned / (float)AssignedItem.StarLevelCost;
        }

        // == Callbacks == //
        public void OnEvolveButton()
        {
            App.Data.Armoury.EvolveItem(AssignedItemId, (success) => { RefreshUI(); });
        }

        public void OnUpgradeButton()
        {
            App.Data.Armoury.UpgradeItem(AssignedItemId, (success) => { RefreshUI(); });
        }
    }
}