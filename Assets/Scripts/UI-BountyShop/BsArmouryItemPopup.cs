
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Bounty
{
    using GM.UI;

    using TMPro;

    [System.Serializable]
    struct ItemImagePair
    {
        public Image Coloured;
        public Image Shadow;

        public Sprite sprite { set { Coloured.sprite = Shadow.sprite = value; } }
    }


    public class BsArmouryItemPopup : MonoBehaviour
    {
        [SerializeField] ItemImagePair itemImages;

        [SerializeField] TMP_Text titleText;
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
            itemImages.sprite = ItemGameData.Icon;

            titleText.text = ItemGameData.ArmouryItem.Name;
            purchaseCostText.text = $"{ItemGameData.PurchaseCost}";

            rating.Show(ItemGameData.ArmouryItem.Tier);
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
