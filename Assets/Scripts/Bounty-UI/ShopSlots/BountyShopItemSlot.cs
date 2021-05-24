using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public class BountyShopItemSlot : AbstractBountyShopSlot
    {
        [SerializeField] Text purchaseQuantityText;

        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            outStockObject.SetActive(!BountyShopManager.Instance.InStock(ServerItemData.ID));

            purchaseCostText.text = ServerItemData.PurchaseCost.ToString();
        }

        protected override void OnItemAssigned()
        {
            base.OnItemAssigned();

            purchaseQuantityText.text = string.Format("x{0}", ServerItemData.QuantityPerPurchase);
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            BountyShopManager.Instance.PurchaseItem(ServerItemData.ID, (_) => { });
        }
    }
}