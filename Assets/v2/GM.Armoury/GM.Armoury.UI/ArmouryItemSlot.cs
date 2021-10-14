using TMPro;
using UnityEngine;
using GM.Utils;
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
        public TMP_Text LevelText;
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
            ItemTierDisplayConfig config = ArmouryUtils.GetDisplayConfig(AssignedItem.Tier);

            TierText.color = config.Colour;
            TierText.text = config.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void FixedUpdate()
        {
            LevelText.text = $"Level {AssignedItem.CurrentLevel}";
            OwnedText.text = $"{AssignedItem.CurrentNumOwned} / {AssignedItem.EvolveCost}";

            EvolveProgressSlider.value = AssignedItem.CurrentNumOwned / (float)AssignedItem.EvolveCost;
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
