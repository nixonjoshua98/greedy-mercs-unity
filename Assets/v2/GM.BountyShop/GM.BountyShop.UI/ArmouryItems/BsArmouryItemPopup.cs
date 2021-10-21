using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BsArmouryItemPopup : BSArmouryItemObject
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text TierText;
        public TMP_Text PurchaseCostText;
        [Space]
        public Image IconImage;

        Action purchaseCallback; // Can be null

        public void Assign(Data.BountyShopArmouryItem item, Action _purchaseCallback)
        {
            purchaseCallback = _purchaseCallback;

            Assign(item);
        }

        protected override void OnAssignedItem()
        {
            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();
            NameText.text = AssignedItem.ItemName;
            IconImage.sprite = AssignedItem.Icon;           
            TierText.color = AssignedItem.Item.Config.Colour;
            TierText.text = AssignedItem.Item.Config.DisplayText;
        }

        // == Callbacks == //
        public void OnPurchaseButton()
        {
            App.Data.BountyShop.PurchaseItem(AssignedItem.Id, (success) =>
            {
                purchaseCallback?.Invoke();

                if (!AssignedItem.InStock)
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}
