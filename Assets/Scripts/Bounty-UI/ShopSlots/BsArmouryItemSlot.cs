using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using GM.Armoury;

    public class BsArmouryItemSlot : AbstractBountyShopSlot
    {
        protected new BsArmouryItemData ServerItemData { get { return BountyShopManager.Instance.ServerData.GetArmouryItem(_itemId); } }

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