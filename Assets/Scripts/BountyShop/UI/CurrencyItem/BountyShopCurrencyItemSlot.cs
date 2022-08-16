using SRC.BountyShop.Data;
using SRC.BountyShop.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.BountyShop.UI
{
    public class BountyShopCurrencyItemSlot : AbstractBountyShopItemSlot
    {
        [Header("Components")]
        [SerializeField] Image ItemImage;
        [Space]
        [SerializeField] TMP_Text CostText;
        [SerializeField] TMP_Text QuantityText;
        [Space]
        [SerializeField] Button Button;

        BountyShopCurrencyItem ShopItem;

        public void Set(BountyShopCurrencyItem item)
        {
            ShopItem = item;

            CostText.text = item.PurchaseCost.ToString();
            QuantityText.text = $"{item.Quantity}x";

            ItemImage.sprite = item.Item.Icon;

            UpdateDynamicUI();
        }

        void UpdateDynamicUI()
        {
            Button.interactable = ShopItem.InStock;
            CostText.text = ShopItem.InStock ? ShopItem.PurchaseCost.ToString() : "SOLD";
        }

        void OnItemPurchased(bool success, PurchaseCurrencyResponse response)
        {
            UpdateDynamicUI();
        }

        public void Button_OnClick()
        {
            var modal = InstantiateModal<BountyShopCurrencyItemModal>(PurchaseModalObject);

            modal.Set(ShopItem, OnItemPurchased);
        }
    }
}
