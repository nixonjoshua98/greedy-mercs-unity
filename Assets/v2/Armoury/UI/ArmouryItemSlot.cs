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
        public TMP_Text LevelText;
        public TMP_Text NameText;
        public TMP_Text BonusText;
        [Space]
        public Image IconImage;

        public override void AssignItem(int itemId)
        {
            base.AssignItem(itemId);

            SetStaticElements();
        }

        private void SetStaticElements()
        {
            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        private void FixedUpdate()
        {
            LevelText.text = FormatLevel(AssignedItem.CurrentLevel);
            BonusText.text = Format.Bonus(AssignedItem.BonusType, AssignedItem.BonusValue);
        }

        public void OnPopupButton()
        {
            InstantiateUI<ArmouryItemPopup>(PopupObject).AssignItem(AssignedItem.Id);
        }
    }
}
