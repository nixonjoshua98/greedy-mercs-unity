using UnityEngine;
using UnityEngine.UI;
using GM.BountyShop.Data;

using TMPro;

namespace GM.Bounties
{
    public class BountyShopArmouryItemSlot : Core.GMMonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text tierText;
        [Space]
        [SerializeField] GameObject itemPopupObject;
        [Space]
        [SerializeField] GameObject soldOutChild;

        string _itemId;

        BountyShopArmouryItemData Item => App.Data.BountyShop.GetArmouryItem(_itemId);


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
            itemIcon.sprite = Item.Icon;
            tierText.text   = $"{Item.ItemTier + 1}";
        }


        void UpdateInterfaceElements()
        {
            soldOutChild.SetActive(!Item.InStock);
        }


        // = = = Callbacks = = = //
        public void OnButtonClick()
        {
            CanvasUtils.Instantiate<BountyShopArmouryItemPurchasePopup>(itemPopupObject).Setup(_itemId);
        }
    }
}
