using GM.BountyShop.Data;
using GM.BountyShop.Requests;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GM.UI;
using GM.Common;

namespace GM.BountyShop.UI
{
    public class BountyShopCurrencyItemModal : AbstractBountyShopItemModal
    {
        [Header("Components")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text OwnedText;
        [SerializeField] TMP_Text CostText;
        [Space]
        [SerializeField] Image Icon;

        [Header("Buttons")]
        [SerializeField] Button CancelButton;
        [Space]
        [SerializeField] Button PurchaseButton;
        [SerializeField] TMP_Text PurchaseButtonText;
        [SerializeField] TypeWriter PurchaseButtonTextTypeWriter;

        BountyShopCurrencyItem ShopItem;
        Action<bool, PurchaseCurrencyResponse> PurchaseCallback;

        public void Set(BountyShopCurrencyItem item, Action<bool, PurchaseCurrencyResponse> purchaseCallback)
        {
            ShopItem = item;
            PurchaseCallback = purchaseCallback;

            NameText.text = item.Item.DisplayName;
            OwnedText.text = $"Owned <color=white>{App.Inventory.GetCurrencyString(item.CurrencyType)}</color>";
            CostText.text = item.PurchaseCost.ToString();

            Icon.sprite = item.Item.Icon;

            ShowInnerPanel();
        }

        protected override void UpdatePurchaseUI()
        {
            CancelButton.interactable = !IsSendingRequest;
            PurchaseButton.interactable = !IsSendingRequest;

            PurchaseButtonTextTypeWriter.enabled = IsSendingRequest;

            if (!IsSendingRequest)
                CostText.text = ShopItem.PurchaseCost.ToString();
        }

        public void Purchase()
        {
            IsSendingRequest = true;

            App.BountyShop.PurchaseCurrencyItem(ShopItem.ID, (success, resp) =>
            {
                IsSendingRequest = false;

                PurchaseCallback.Invoke(success, resp);

                if (success)
                {
                    Close();
                }
                else
                {
                    Modals.ShowServerError(resp);
                }
            });
        }
    }
}
