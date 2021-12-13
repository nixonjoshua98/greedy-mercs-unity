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

        void SetStaticElements()
        {
            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }

        void FixedUpdate()
        {
            LevelText.text = FormatLevel(AssignedItem.CurrentLevel);
            BonusText.text = GetBonusText();
        }

        public void OnPopupButton()
        {
            InstantiateUI<ArmouryItemPopup>(PopupObject).AssignItem(AssignedItem.Id);
        }
    }
}
