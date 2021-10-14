using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GM.Armoury.UI_
{
    public class ArmouryItemPopup : ArmouryItemUIObject
    {
        public TMP_Text TierText;
        public TMP_Text NameText;
        [Space]
        [Space]
        public Image IconImage;

        public override void AssignItem(int itemId)
        {
            base.AssignItem(itemId);

            TierText.color = AssignedItem.DisplayConfig.Colour;
            TierText.text = AssignedItem.DisplayConfig.DisplayText;

            IconImage.sprite = AssignedItem.Icon;
            NameText.text = AssignedItem.ItemName;
        }
    }
}
