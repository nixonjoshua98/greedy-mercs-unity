using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.BountyShop
{
    public class BsArmouryItemSlot : AbstractBountyShopSlot
    {
        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            outStockObject.SetActive(!BountyShopManager.Instance.InStock(ServerItemData.ID));

            purchaseCostText.text = ServerItemData.PurchaseCost.ToString();
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            BountyShopManager.Instance.PurchaseArmouryItem(ServerItemData.ID, (_) => { });
        }
    }
}