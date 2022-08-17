using SRC.BountyShop.Data;
using SRC.BountyShop.Requests;
using SRC.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.BountyShop.UI
{
    public class BountyShopArmouryItemPopup : AbstractBountyShopItemModal
    {
        [Header("Components")]
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text CostText;
        [Space]
        [SerializeField] private Image Icon;
        [SerializeField] private Image IconBackground;
        [SerializeField] private Image IconFrame;

        [Header("Buttons")]
        [SerializeField] private Button CloseButton;
        [SerializeField] private Button PurchaseButton;
        [Space]
        [SerializeField] private TypeWriter PurchaseButtonTextTypeWriter;
        private BountyShopArmouryItem ShopItem;
        private Action<bool, PurchaseArmouryItemResponse> PurchaseCallback;

        public void Set(BountyShopArmouryItem item, Action<bool, PurchaseArmouryItemResponse> purchaseCallback)
        {
            ShopItem = item;
            PurchaseCallback = purchaseCallback;

            NameText.text = item.Item.Name;
            CostText.text = item.PurchaseCost.ToString();

            Icon.sprite = item.Item.Icon;
            IconFrame.color = item.Item.GradeConfig.FrameColour;
            IconBackground.sprite = item.Item.GradeConfig.BackgroundSprite;

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

            App.BountyShop.PurchaseArmouryItem(ShopItem.ID, (success, resp) =>
            {
                IsSendingRequest = false;

                PurchaseCallback.Invoke(success, resp);

                if (!success)
                    GMLogger.Error(resp.Message);
                else
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}