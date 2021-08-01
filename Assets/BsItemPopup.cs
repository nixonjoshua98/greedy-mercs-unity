using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using TMPro;

    public class BsItemPopup : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text quantityText;

        string _itemId;

        BountyShopItem ItemGameData => UserData.Get.BountyShop.GetItem(_itemId);

        public void Setup(string id)
        {
            _itemId = id;

            SetInterfaceElements();
        }

        void SetInterfaceElements()
        {
            titleText.text = $"Buy {ItemGameData.ItemData.DisplayName}?";
            itemIcon.sprite = ItemGameData.Icon;
            quantityText.text = $"x{ItemGameData.QuantityPerPurchase}";
        }
    }
}
