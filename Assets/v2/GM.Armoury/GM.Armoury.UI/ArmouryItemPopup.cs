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

            TierText.color = AssignedItem.DisplayConfig.Colour;
            TierText.text = AssignedItem.DisplayConfig.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void FixedUpdate()
        {
            LevelText.text = $"Level: {AssignedItem.CurrentLevel}";
            EvolveButton.interactable = AssignedItem.ReadyToEvolve;
            OwnedText.text = $"{AssignedItem.CurrentNumOwned} / {AssignedItem.EvolveCost}";
            EvolveProgressSlider.value = AssignedItem.CurrentNumOwned / (float)AssignedItem.EvolveCost;
        }

        // == Callbacks == //
        public void OnEvolveButton()
        {
            App.Data.Armoury.EvolveItem(AssignedItemId, (success) => {  });
        }

        public void OnUpgradeButton()
        {
            App.Data.Armoury.UpgradeItem(AssignedItemId, (success) => {  });
        }
    }
}