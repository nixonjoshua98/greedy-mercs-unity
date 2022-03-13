using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopCurrencyTypeSlot : BountyShopCurrencyTypeUIObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public GameObject OutStockObject;
        public TMP_Text PurchaseCostText;
        public TMP_Text QuantityText;
        public Image IconImage;

        protected override void OnAssignedItem()
        {
            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();
            QuantityText.text = $"x{AssignedItem.QuantityPerPurchase}";

            IconImage.sprite = AssignedItem.Item.Icon;

            CheckAvailability();
        }

        void CheckAvailability()
        {
            OutStockObject.SetActive(!AssignedItem.InStock);
        }

        public void OnClick()
        {
            InstantiateUI<BountyShopCurrencyPopup>(PopupObject).Assign(AssignedItem, () =>
            {
                CheckAvailability();
            });
        }
    }
}
