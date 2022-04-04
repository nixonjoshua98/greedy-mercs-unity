using GM.BountyShop.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopCurrencyPopup : BountyShopCurrencyTypeUIObject
    {
        [Header("References")]
        public TMP_Text NameText;
        public TMP_Text QuantityText;
        [Space]
        public Image IconImage;
        private Action purchaseCallback; // Can be null

        public void Assign(BountyShopCurrencyItemModel item, Action _purchaseCallback)
        {
            Assign(item);

            purchaseCallback = _purchaseCallback;
        }

        protected override void OnAssignedItem()
        {
            IconImage.sprite = AssignedItem.Item.Icon;

            NameText.text = AssignedItem.Item.DisplayName;
            QuantityText.text = $"x{AssignedItem.Quantity}";
        }


        public void OnPurchaseButton()
        {
            App.BountyShop.PurchaseCurrencyItem(AssignedItem.ID, (success) =>
            {
                purchaseCallback?.Invoke();

                if (!AssignedItem.InStock)
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}
