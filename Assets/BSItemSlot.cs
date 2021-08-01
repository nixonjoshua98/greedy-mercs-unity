using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using TMPro;

    public class BSItemSlot : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text quantityText;
        [Space]
        [SerializeField] GameObject itemPopupObject;

        string _itemId;

        BountyShopItem ItemGameData => UserData.Get.BountyShop.GetItem(_itemId);
        
        public void Setup(string id)
        {
            _itemId = id;

            SetInterfaceElements();
        }

        void SetInterfaceElements()
        {
            itemIcon.sprite     = ItemGameData.Icon;
            quantityText.text   = $"x{ItemGameData.QuantityPerPurchase}";
        }

        
        // = = = Callbacks = = = //
        public void OnButtonClick()
        {
            GameObject o = CanvasUtils.Instantiate(itemPopupObject);

            o.GetComponent<BsItemPopup>().Setup(_itemId);
        }

    }
}
