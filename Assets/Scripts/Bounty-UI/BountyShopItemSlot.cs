using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public class BountyShopItemSlot : MonoBehaviour
    {
        [Header("Components - UI")]
        [SerializeField] Image itemIcon;
        [Space]
        [SerializeField] Text itemNameText;
        [SerializeField] Text purchaseCostText;

        BountyShopItem assignedSlotItem;


        bool _isUpdatingUi = false;

        public void SetItem(BountyShopItem item)
        {
            assignedSlotItem = item; // Set which item this slot will represent

            SetIcon();
            SetName();

            _isUpdatingUi = true;
        }

        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            purchaseCostText.text = assignedSlotItem.PurchaseCost.ToString();
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

        // = = =
        void SetIcon()
        {
            string key = "blue_gem_icon";

            switch (assignedSlotItem.ItemType)
            {
                case BountyShopItemType.FLAT_BLUE_GEM:
                    key = "blue_gem_icon";
                    break;

                case BountyShopItemType.FLAT_AP:
                    key = "iron_icon";
                    break;

                case BountyShopItemType.FLAT_BP:
                    key = "bounty_point_icon";
                    break;
            }

            itemIcon.sprite = ResourceManager.LoadSprite("Icons", key);
        }

        void SetName()
        {
            itemNameText.text = assignedSlotItem.ItemType.ToString();
        }
    }
}