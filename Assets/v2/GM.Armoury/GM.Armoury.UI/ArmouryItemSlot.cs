using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI_
{
    public class ArmouryItemSlot : ArmouryItemUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;
        [Space]
        public TMP_Text TierText;
        public TMP_Text NameText;
        public TMP_Text OwnedText;
        [Space]
        public Slider EvolveProgressSlider;
        public Image EvolveProgressSliderFill;
        [Space]
        public Image IconImage;

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
            OwnedText.text = $"{AssignedItem.NumOwned} / {AssignedItem.MergeCost}";

            EvolveProgressSlider.value = AssignedItem.NumOwned / (float)AssignedItem.MergeCost;
        }

        // == Callbacks == //
        public void OnEvolveProgressSliderValueChanged()
        {
            EvolveProgressSliderFill.color = Color.Lerp(Color.red, new Color(0.0f, 0.8f, 0.0f), EvolveProgressSlider.value);
        }

        public void OnPopupButton()
        {
            CanvasUtils.Instantiate<ArmouryItemPopup>(PopupObject).
                AssignItem(AssignedItemId);
        }
    }
}
