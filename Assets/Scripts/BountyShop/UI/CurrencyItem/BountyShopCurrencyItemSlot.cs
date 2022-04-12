using GM.BountyShop.Data;
using GM.BountyShop.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopCurrencyItemSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PurchaseModalObject;

        [Header("Components")]
        [SerializeField] Image ItemImage;
        [Space]
        [SerializeField] TMP_Text CostText;
        [Space]
        [SerializeField] Button Button;

        BountyShopCurrencyItem ShopItem;

        public void Set(BountyShopCurrencyItem item)
        {
            ShopItem = item;

            CostText.text = item.PurchaseCost.ToString();

            ItemImage.sprite = item.Item.Icon;
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
            InstantiateUI<BountyShopCurrencyItemModal>(PurchaseModalObject).Set(ShopItem, OnItemPurchased);
        }
    }
}
