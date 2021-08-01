using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Bounty
{
    using TMPro;

    public class BsArmouryItemSlot : ExtendedMonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text tierText;
        [Space]
        [SerializeField] GameObject itemPopupObject;
        [Space]
        [SerializeField] GameObject soldOutChild;

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
            itemIcon.sprite = ItemGameData.Icon;
            tierText.text = $"{ItemGameData.ArmouryItem.Tier}";
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
            CanvasUtils.Instantiate<BsArmouryItemPopup>(itemPopupObject).Setup(_itemId);
        }
    }
}
