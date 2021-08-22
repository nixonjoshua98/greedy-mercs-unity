
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    public class BsItemPopup : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] Text titleText;
        [SerializeField] Text quantityText;
        [SerializeField] Text purchaseCostText;
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

            titleText.text = ItemGameData.ItemData.DisplayName.ToUpper();
            quantityText.text = $"X{ItemGameData.QuantityPerPurchase}";
            purchaseCostText.text = $"{ItemGameData.PurchaseCost}";
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
