using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public class BountyShopItemSlot : MonoBehaviour
    {
        [Header("Children")]
        [SerializeField] GameObject outStockObject;

        [Header("Components - UI")]
        [SerializeField] Image itemIcon;
        [Space]
        [SerializeField] Text purchaseCostText;

        int _itemId;
        bool _isUpdatingUi = false;

        BountyShopItemData ServerItemData { get { return GreedyMercs.StaticData.BountyShop.GetItem(_itemId); } }

        public void SetItemID(int itemId)
        {
            _itemId         = itemId;
            _isUpdatingUi   = true;

            SetIcon();
        }

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
            BountyShopManager.Instance.PurchaseItem(ServerItemData.ID, OnPurchasedItem);
        }

        // = = = Server Callbacks ===
        void OnPurchasedItem(bool hasPurchased)
        {

        }

        // = = =
        void SetIcon()
        {
            string key = "blue_gem_icon";

            switch (ServerItemData.ItemType)
            {
                case BountyShopItemType.FLAT_BLUE_GEM:
                    key = "blue_gem_icon";
                    break;

                case BountyShopItemType.FLAT_AP:
                    key = "iron_icon";
                    break;
            }

            itemIcon.sprite = ResourceManager.LoadSprite("Icons", key);
        }
    }
}