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
        [SerializeField] private Image ItemImage;
        [Space]
        [SerializeField] private TMP_Text CostText;
        [SerializeField] private TMP_Text QuantityText;
        [Space]
        [SerializeField] private Button Button;
        private BountyShopCurrencyItem ShopItem;

        public void Set(BountyShopCurrencyItem item)
        {
            ShopItem = item;

            CostText.text = item.PurchaseCost.ToString();
            QuantityText.text = $"{item.Quantity}x";

            ItemImage.sprite = item.Item.Icon;

            UpdateDynamicUI();
        }

        private void UpdateDynamicUI()
        {
            Button.interactable = ShopItem.InStock;
            CostText.text = ShopItem.InStock ? ShopItem.PurchaseCost.ToString() : "SOLD";
        }

        private void OnItemPurchased(bool success, PurchaseCurrencyResponse response)
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
