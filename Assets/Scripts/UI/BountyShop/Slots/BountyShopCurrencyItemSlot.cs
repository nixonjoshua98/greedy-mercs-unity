using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using GM.BountyShop.Data;

using TMPro;

namespace GM.Bounties
{
    public class BountyShopCurrencyItemSlot : ExtendedMonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text quantityText;
        [Space]
        [SerializeField] GameObject itemPopupObject;
        [Space]
        [SerializeField] GameObject soldOutChild;

        string _itemId;

        BountyShopCurrencyItemData Item => App.Data.BountyShop.GetCurrencyItem(_itemId);
        
        public void Setup(string id)
        {
            _itemId = id;

            SetInterfaceElements();
            UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            itemIcon.sprite     = Item.Icon;
            quantityText.text   = $"X{Item.QuantityPerPurchase}";
        }


        void UpdateInterfaceElements()
        {
            soldOutChild.SetActive(!Item.InStock);
        }

        protected override void PeriodicUpdate()
        {
            UpdateInterfaceElements();
        }


        // = = = Callbacks = = = //
        public void OnButtonClick()
        {
            CanvasUtils.Instantiate<BountyShopItemPurchasePopup>(itemPopupObject).Setup(_itemId);
        }
    }
}
