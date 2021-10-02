
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using GM.BountyShop.Data;

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

        BountyShopCurrencyItemData Item => App.Data.BountyShop.GetCurrencyItem(ItemID);

        protected override void SetInterfaceElements()
        {
            itemIcon.sprite = Item.Icon;

            titleText.text = Item.DisplayName;
            quantityText.text = $"x{Item.QuantityPerPurchase}";
            purchaseCostText.text = $"{Item.PurchaseCost}";
        }


        protected override void UpdateInterfaceElements()
        {
            purchaseButton.interactable = Item.InStock;
        }


        public void OnPurchaseButton()
        {
            App.Data.BountyShop.PurchaseItem(ItemID, (success) =>
            {
                UpdateInterfaceElements();
                DestroyWhenOutOfStock();
            });
        }
    }
}
