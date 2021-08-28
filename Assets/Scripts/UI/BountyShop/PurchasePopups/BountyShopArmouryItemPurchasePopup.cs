
using UnityEngine;
using UnityEngine.UI;

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

        BountyShopArmouryItem ItemUserData => UserData.Get.BountyShop.GetArmouryItem(ItemID);


        protected override void SetInterfaceElements()
        {
            itemImages.sprite       = ItemUserData.Icon;
            itemNameText.text       = ItemUserData.ArmouryItem.Name.ToUpper();
            purchaseCostText.text   = $"{ItemUserData.PurchaseCost}";

            rating.Show(ItemUserData.ArmouryItem.Tier + 1);
        }


        protected override void UpdateInterfaceElements()
        {
            purchaseButton.interactable = UserData.Get.BountyShop.InStock(ItemID);
        }


        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseArmouryItem(ItemID, (success) =>
            {
                UpdateInterfaceElements();
                DestroyWhenOutOfStock();
            });
        }
    }
}
