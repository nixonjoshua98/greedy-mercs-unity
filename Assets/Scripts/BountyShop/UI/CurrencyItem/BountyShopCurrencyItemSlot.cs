using GM.BountyShop.Data;
using GM.BountyShop.Requests;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] GameObject SoldOverlay;

        BountyShopCurrencyItem ShopItem;

        public void Set(BountyShopCurrencyItem item)
        {
            ShopItem = item;

            CostText.text = item.PurchaseCost.ToString();

            ItemImage.sprite = item.Item.Icon;

            SoldOverlay.SetActive(!item.InStock);
        }

        void OnItemPurchased(bool success, PurchaseCurrencyResponse response)
        {
            SoldOverlay.SetActive(!ShopItem.InStock);
        }

        public void Button_OnClick()
        {
            InstantiateUI<BountyShopCurrencyItemModal>(PurchaseModalObject).Set(ShopItem, OnItemPurchased);
        }
    }
}
