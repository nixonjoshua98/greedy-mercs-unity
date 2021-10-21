using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BSArmouryItemSlot : BSArmouryItemObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;
        
        [Header("References")]
        public TMP_Text TierText;
        public TMP_Text PurchaseCostText;
        [Space]
        public Image IconImage;

        protected override void OnAssignedItem()
        {
            TierText.color = AssignedItem.ItemData.Config.Colour;
            TierText.text = AssignedItem.ItemData.Config.DisplayText;

            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();

            IconImage.sprite = AssignedItem.Icon;
        }

        // == Callbacks == //

        public void OnClick()
        {
            InstantiateUI<BsArmouryItemPopup>(PopupObject).Assign(AssignedItem);
        }
    }
}
