using SRC.BountyShop.Data;
using SRC.BountyShop.Requests;
using SRC.Common.Enums;
using SRC.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.BountyShop.UI
{
    public class BountyShopCurrencyItemModal : AbstractBountyShopItemModal
    {
        [Header("Components")]
        [SerializeField] private GenericGradeItem GradeSlot;
        [SerializeField] private TMP_Text TitleText;
        [SerializeField] private TMP_Text CostText;

        [Header("Buttons")]
        [SerializeField] private Button CloseButton;
        [SerializeField] private Button PurchaseButton;
        [Space]
        [SerializeField] private TypeWriter PurchaseButtonTextTypeWriter;
        private BountyShopCurrencyItem ShopItem;
        private Action<bool, PurchaseCurrencyResponse> PurchaseCallback;

        public void Set(BountyShopCurrencyItem item, Action<bool, PurchaseCurrencyResponse> purchaseCallback)
        {
            ShopItem = item;
            PurchaseCallback = purchaseCallback;

            TitleText.text = $"<color=orange>{item.Quantity}x</color> {item.Item.DisplayName}";
            CostText.text = item.PurchaseCost.ToString();

            GradeSlot.Intialize(Rarity.Zero, item.Icon);

            ShowInnerPanel();
        }

        protected override void UpdatePurchaseUI()
        {
            CloseButton.interactable = !IsSendingRequest;
            PurchaseButton.interactable = !IsSendingRequest;

            PurchaseButtonTextTypeWriter.enabled = IsSendingRequest;

            if (!IsSendingRequest)
                CostText.text = ShopItem.PurchaseCost.ToString();
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }

        public void OnPurchaseButton()
        {
            IsSendingRequest = true;

            App.BountyShop.PurchaseCurrencyItem(ShopItem.ID, (success, resp) =>
            {
                IsSendingRequest = false;

                PurchaseCallback.Invoke(success, resp);

                if (success)
                {
                    Destroy(gameObject);
                }
                else
                {
                    GMLogger.Error(resp.Message);
                }
            });
        }
    }
}
