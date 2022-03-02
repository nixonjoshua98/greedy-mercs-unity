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

        Action purchaseCallback; // Can be null

        public void Assign(BountyShopCurrencyItemModel item, Action _purchaseCallback)
        {
            Assign(item);

            purchaseCallback = _purchaseCallback;
        }

        protected override void OnAssignedItem()
        {
            IconImage.sprite = AssignedItem.Item.Icon;

            NameText.text = AssignedItem.Item.DisplayName;
            QuantityText.text = $"x{AssignedItem.QuantityPerPurchase}";
        }


        public void OnPurchaseButton()
        {
            App.GMData.BountyShop.PurchaseCurrencyItem(AssignedItem.Id, (success) =>
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
