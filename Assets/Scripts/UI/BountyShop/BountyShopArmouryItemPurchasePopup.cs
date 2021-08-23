
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace GM.Bounty
{
    using GM.UI;

    [System.Serializable]
    struct ItemImagePair
    {
        public Image Coloured;
        public Image Shadow;

        public Sprite sprite { set { Coloured.sprite = Shadow.sprite = value; } }
    }


    public class BountyShopArmouryItemPurchasePopup : MonoBehaviour
    {
        [SerializeField] ItemImagePair itemImages;

        [SerializeField] TMP_Text itemNameText;
        [SerializeField] TMP_Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;
        [Space]
        [SerializeField] StarRatingController rating;

        string _itemId;

        BountyShopArmouryItem ItemGameData => UserData.Get.BountyShop.GetArmouryItem(_itemId);

        public void Setup(string id)
        {
            _itemId = id;

            SetInterfaceElements();
            UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            itemImages.sprite       = ItemGameData.Icon;
            itemNameText.text          = ItemGameData.ArmouryItem.Name.ToUpper();
            purchaseCostText.text   = $"{ItemGameData.PurchaseCost}";

            rating.Show(ItemGameData.ArmouryItem.Tier + 1);
        }


        void UpdateInterfaceElements()
        {
            purchaseButton.interactable = UserData.Get.BountyShop.InStock(_itemId);
        }


        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseArmouryItem(_itemId, (success) =>
            {
                UpdateInterfaceElements();
            });
        }
    }
}
