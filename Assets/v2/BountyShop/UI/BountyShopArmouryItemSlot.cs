using GM.BountyShop.Data;
using GM.HTTP.Requests.BountyShop;
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
        [SerializeField] GameObject SoldOverlay;

        BountyShopArmouryItem ShopItem;

        public void Set(BountyShopArmouryItem item)
        {
            ShopItem = item;

            CostText.text = item.PurchaseCost.ToString();

            ItemImage.sprite = item.Item.Icon;
            IconFrame.color = item.Item.GradeConfig.FrameColour;
            BackgroundImage.sprite = item.Item.GradeConfig.BackgroundSprite;

            SoldOverlay.SetActive(!item.InStock);
        }

        void OnItemPurchased(bool success, PurchaseArmouryItemResponse response)
        {
            SoldOverlay.SetActive(!ShopItem.InStock);
        }

        public void Button_OnClick()
        {
            InstantiateUI<BountyShopArmouryItemModal>(PurchaseModalObject).Set(ShopItem, OnItemPurchased);
        }
    }
}
