using SRC.BountyShop.Data;
using SRC.BountyShop.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.BountyShop.UI
{
    public class BountyShopArmouryItemSlot : AbstractBountyShopItemSlot
    {
        [Header("Components")]
        [SerializeField] private Image ItemImage;
        [SerializeField] private Image IconFrame;
        [SerializeField] private Image BackgroundImage;
        [Space]
        [SerializeField] private TMP_Text CostText;
        [Space]
        [SerializeField] private Button Button;
        private BountyShopArmouryItem ShopItem;

        public void Set(BountyShopArmouryItem item)
        {
            ShopItem = item;

            ItemImage.sprite = item.Item.Icon;
            IconFrame.color = item.Item.GradeConfig.FrameColour;
            BackgroundImage.sprite = item.Item.GradeConfig.BackgroundSprite;

            UpdateDynamicUI();
        }

        private void UpdateDynamicUI()
        {
            Button.interactable = ShopItem.InStock;
            CostText.text = ShopItem.InStock ? ShopItem.PurchaseCost.ToString() : "SOLD";
        }

        private void OnItemPurchased(bool success, PurchaseArmouryItemResponse response)
        {
            UpdateDynamicUI();
        }

        public void Button_OnClick()
        {
            var modal = InstantiateModal<BountyShopArmouryItemPopup>(PurchaseModalObject);

            modal.Set(ShopItem, OnItemPurchased);
        }
    }
}
