using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    public class ArmouryItemPopup : ArmouryItemUIObject
    {
        [Header("References")]
        public GM.UI.StarsController Stars;
        [Space]
        public Button EvolveButton;
        [Space]
        public TMP_Text TierText;
        public TMP_Text NameText;
        public TMP_Text LevelText;
        public TMP_Text OwnedText;
        public TMP_Text DamageText;
        [Space]
        public Image IconImage;
        public Slider EvolveProgressSlider;

        protected override void OnAssignedItem()
        {
            SetStaticUIElements();
            RefreshUI();
        }

        void SetStaticUIElements()
        {
            TierText.color = AssignedItem.Config.Colour;
            TierText.text = AssignedItem.Config.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void RefreshUI()
        {
            Stars.Show(AssignedItem.CurrentMergeLevel);

            LevelText.text = $"Level {AssignedItem.CurrentLevel}";
            EvolveButton.interactable = AssignedItem.CanMerge;
            OwnedText.text = $"{AssignedItem.NumOwned} / {AssignedItem.MergeCost}";
            EvolveProgressSlider.value = AssignedItem.NumOwned / (float)AssignedItem.MergeCost;
            DamageText.text = $"Damage <color=orange>{Format.Percentage(AssignedItem.WeaponDamage)}</color>";
        }

        // == Callbacks == //
        public void OnMergeButton()
        {
            App.Data.Armoury.EvolveItem(AssignedItemId, (success) => { RefreshUI(); });
        }

        public void OnUpgradeButton()
        {
            App.Data.Armoury.UpgradeItem(AssignedItemId, (success) => { RefreshUI(); });
        }
    }
}