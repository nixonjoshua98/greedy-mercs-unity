
using UnityEngine;
using UnityEngine.UI;
using GM.BountyShop.Data;

using TMPro;

namespace GM.Bounties
{
    using GM.UI;

    [System.Serializable]
    struct ItemImagePair
    {
        public Image Coloured;
        public Image Shadow;

        public Sprite sprite { set { Coloured.sprite = Shadow.sprite = value; } }
    }


    public class BountyShopArmouryItemPurchasePopup : AbstractBountyShopItemPurchasePopup
    {
        [SerializeField] ItemImagePair itemImages;

        [SerializeField] TMP_Text itemNameText;
        [SerializeField] TMP_Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;
        [Space]
        [SerializeField] StarRatingController rating;

        BountyShopArmouryItemData Item => App.Data.BountyShop.GetArmouryItem(ItemID);


        protected override void SetInterfaceElements()
        {
            itemImages.sprite       = Item.Icon;
            itemNameText.text       = Item.ItemName.ToUpper();
            purchaseCostText.text   = $"{Item.PurchaseCost}";

            rating.Show(Item.ItemTier + 1);
        }


        protected override void UpdateInterfaceElements()
        {
            purchaseButton.interactable = Item.InStock;
        }


        public void OnPurchaseButton()
        {
            App.Data.BountyShop.PurchaseArmouryItem(ItemID, (success) =>
            {
                UpdateInterfaceElements();
                DestroyWhenOutOfStock();
            });
        }
    }
}
