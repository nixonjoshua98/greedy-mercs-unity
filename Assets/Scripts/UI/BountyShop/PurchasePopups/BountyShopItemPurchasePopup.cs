
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace GM.Bounties
{
    public class BountyShopItemPurchasePopup : AbstractBountyShopItemPurchasePopup
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text quantityText;
        [SerializeField] TMP_Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;

        BountyShopItem ItemGameData => UserData.Get.BountyShop.GetItem(ItemID);

        protected override void SetInterfaceElements()
        {
            itemIcon.sprite = ItemGameData.Icon;

            titleText.text          = ItemGameData.ItemData.DisplayName;
            quantityText.text       = $"x{ItemGameData.QuantityPerPurchase}";
            purchaseCostText.text   = $"{ItemGameData.PurchaseCost}";
        }


        protected override void UpdateInterfaceElements()
        {
            purchaseButton.interactable = UserData.Get.BountyShop.InStock(ItemID);
        }


        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseItem(ItemID, (success) =>
            {
                UpdateInterfaceElements();
                DestroyWhenOutOfStock();
            });
        }
    }
}
