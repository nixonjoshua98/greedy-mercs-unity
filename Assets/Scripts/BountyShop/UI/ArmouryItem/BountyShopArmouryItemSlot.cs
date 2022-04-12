using GM.BountyShop.Data;
using GM.BountyShop.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopArmouryItemSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PurchaseModalObject;

        [Header("Components")]
        [SerializeField] Image ItemImage;
        [SerializeField] Image IconFrame;
        [SerializeField] Image BackgroundImage;
        [Space]
        [SerializeField] TMP_Text CostText;
        [Space]
        [SerializeField] Button Button;

        BountyShopArmouryItem ShopItem;

        public void Set(BountyShopArmouryItem item)
        {
            ShopItem = item;

            ItemImage.sprite = item.Item.Icon;
            IconFrame.color = item.Item.GradeConfig.FrameColour;
            BackgroundImage.sprite = item.Item.GradeConfig.BackgroundSprite;

            UpdateDynamicUI();
        }

        void UpdateDynamicUI()
        {
            Button.interactable = ShopItem.InStock;
            CostText.text = ShopItem.InStock ? ShopItem.PurchaseCost.ToString() : "SOLD";
        }

        void OnItemPurchased(bool success, PurchaseArmouryItemResponse response)
        {
            UpdateDynamicUI();
        }

        public void Button_OnClick()
        {
            InstantiateUI<BountyShopArmouryItemModal>(PurchaseModalObject).Set(ShopItem, OnItemPurchased);
        }
    }
}
