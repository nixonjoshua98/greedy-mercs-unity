using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace GM.Bounty
{
    public class BountyShopArmouryItemSlot : MonoBehaviour
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

        void FixedUpdate()
        {
            UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            itemIcon.sprite = ItemGameData.Icon;
            tierText.text   = $"{ItemGameData.ArmouryItem.Tier + 1}";
        }


        void UpdateInterfaceElements()
        {
            soldOutChild.SetActive(!UserData.Get.BountyShop.InStock(_itemId));
        }


        // = = = Callbacks = = = //
        public void OnButtonClick()
        {
            CanvasUtils.Instantiate<BountyShopArmouryItemPurchasePopup>(itemPopupObject).Setup(_itemId);
        }
    }
}
