using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.BountyShop
{
    public class BountyShopSlot : MonoBehaviour
    {
        BountyShopItem assignedSlotItem;

        public void SetItem(BountyShopItem item)
        {
            assignedSlotItem = item; // Set which item this slot will represent
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            BountyShopManager.Instance.PurchaseItem(assignedSlotItem.ID, OnPurchasedItem);
        }

        // = = = Server Callbacks ===
        void OnPurchasedItem(bool hasPurchased)
        {

        }
    }
}