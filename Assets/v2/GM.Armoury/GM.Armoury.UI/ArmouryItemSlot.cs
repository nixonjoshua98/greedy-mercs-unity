using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    public class ArmouryItemSlot : ArmouryItemUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public TMP_Text TierText;
        public TMP_Text NameText;
        public TMP_Text OwnedText;
        [Space]
        public Slider EvolveProgressSlider;
        [Space]
        public Image EvolveProgressSliderFill;
        public Image IconImage;
        [Space]
        public GM.UI.StarsController Stars;

        public override void AssignItem(int itemId)
        {
            base.AssignItem(itemId);

            SetStaticElements();
        }

        void SetStaticElements()
        {
            TierText.color = AssignedItem.Config.Colour;
            TierText.text = AssignedItem.Config.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;

        }

        void FixedUpdate()
        {
            Stars.Show(AssignedItem.CurrentMergeLevel);

            OwnedText.text = $"{AssignedItem.NumOwned} / {AssignedItem.MergeCost}";

            EvolveProgressSlider.value = AssignedItem.NumOwned / (float)AssignedItem.MergeCost;
            EvolveProgressSliderFill.color = Color.Lerp(Common.Colors.Red, Common.Colors.Green, EvolveProgressSlider.value);
        }

        public void OnPopupButton()
        {
            CanvasUtils.Instantiate<ArmouryItemPopup>(PopupObject).
                AssignItem(AssignedItemId);
        }
    }
}
