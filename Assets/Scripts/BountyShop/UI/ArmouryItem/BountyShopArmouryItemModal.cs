using GM.BountyShop.Data;
using GM.BountyShop.Requests;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopArmouryItemModal : AbstractBountyShopItemModal
    {
        [Header("Components")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text OwnedText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text CostText;
        [Space]
        [SerializeField] Image Icon;
        [SerializeField] Image IconBackground;
        [SerializeField] Image IconFrame;

        [Header("Buttons")]
        [SerializeField] Button CancelButton;
        [Space]
        [SerializeField] Button PurchaseButton;
        [SerializeField] TMP_Text PurchaseButtonText;
        [SerializeField] TypeWriter PurchaseButtonTextTypeWriter;

        BountyShopArmouryItem ShopItem;
        Action<bool, PurchaseArmouryItemResponse> PurchaseCallback;

        public void Set(BountyShopArmouryItem item, Action<bool, PurchaseArmouryItemResponse> purchaseCallback)
        {
            ShopItem = item;
            PurchaseCallback = purchaseCallback;

            NameText.text = item.Item.Name;
            OwnedText.text = $"Owned <color=white>{(item.Item.UserOwnsItem ? item.Item.NumOwned : "None")}</color>";
            LevelText.text = $"Level <color=white>{(item.Item.UserOwnsItem ? item.Item.CurrentLevel : "0")}</color>";
            CostText.text = item.PurchaseCost.ToString();

            Icon.sprite = item.Item.Icon;
            IconFrame.color = item.Item.GradeConfig.FrameColour;
            IconBackground.sprite = item.Item.GradeConfig.BackgroundSprite;

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

            App.BountyShop.PurchaseArmouryItem(ShopItem.ID, (success, resp) =>
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