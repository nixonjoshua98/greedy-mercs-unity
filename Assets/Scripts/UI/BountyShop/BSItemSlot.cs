using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    public class BSItemSlot : ExtendedMonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] Text quantityText;
        [Space]
        [SerializeField] GameObject itemPopupObject;
        [Space]
        [SerializeField] GameObject soldOutChild;

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
            itemIcon.sprite     = ItemGameData.Icon;
            quantityText.text   = $"X{ItemGameData.QuantityPerPurchase}";
        }


        void UpdateInterfaceElements()
        {
            soldOutChild.SetActive(!UserData.Get.BountyShop.InStock(_itemId));
        }

        protected override void PeriodicUpdate()
        {
            UpdateInterfaceElements();
        }


        // = = = Callbacks = = = //
        public void OnButtonClick()
        {
            CanvasUtils.Instantiate<BsItemPopup>(itemPopupObject).Setup(_itemId);
        }
    }
}
