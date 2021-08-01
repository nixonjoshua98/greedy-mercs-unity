using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public class BountyShopItemSlot : AbstractBountyShopSlot
    {
        [SerializeField] Text purchaseQuantityText;

        protected override void OnItemAssigned()
        {
            base.OnItemAssigned();

            purchaseQuantityText.text   = string.Format("x{0}", ShopItemData.QuantityPerPurchase);
            purchaseCostText.text       = ShopItemData.PurchaseCost.ToString();
        }

        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            outStockObject.SetActive(!UserData.Get.BountyShop.InStock(ShopItemData.ID));
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseItem(ShopItemData.ID, (_) => { });
        }
    }
}