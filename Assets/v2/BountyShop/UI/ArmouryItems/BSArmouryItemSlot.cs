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
        public GameObject OutStockObject;
        [Space]
        public TMP_Text TierText;
        public TMP_Text PurchaseCostText;
        [Space]
        public Image IconImage;

        protected override void OnAssignedItem()
        {
            TierText.color = AssignedItem.Item.Config.Colour;
            TierText.text = AssignedItem.Item.Config.DisplayText;

            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();

            IconImage.sprite = AssignedItem.Icon;
        }

        // == Callbacks == //

        void OnItemPurchased()
        {
            if (!AssignedItem.InStock)
            {
                OutStockObject.SetActive(true);
            }
        }

        public void OnClick()
        {
            InstantiateUI<BsArmouryItemPopup>(PopupObject).Assign(AssignedItem, OnItemPurchased);
        }
    }
}
