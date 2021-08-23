
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace GM.Bounty
{
    public class BountyShopItemPurchasePopup : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text quantityText;
        [SerializeField] TMP_Text purchaseCostText;
        [Space]
        [SerializeField] Button purchaseButton;

        string _itemId;

        BountyShopItem ItemGameData => UserData.Get.BountyShop.GetItem(_itemId);

        public void Setup(string id)
        {
            _itemId = id;

            SetInterfaceElements();
            UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            itemIcon.sprite = ItemGameData.Icon;

            titleText.text          = ItemGameData.ItemData.DisplayName;
            quantityText.text       = $"x{ItemGameData.QuantityPerPurchase}";
            purchaseCostText.text   = $"{ItemGameData.PurchaseCost}";
        }


        void UpdateInterfaceElements()
        {
            purchaseButton.interactable = UserData.Get.BountyShop.InStock(_itemId);
        }


        public void OnPurchaseButton()
        {
            UserData.Get.BountyShop.PurchaseItem(_itemId, (success) =>
            {
                UpdateInterfaceElements();
            });
        }
    }
}
