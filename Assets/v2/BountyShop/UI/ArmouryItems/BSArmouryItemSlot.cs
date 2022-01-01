using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BSArmouryItemSlot : BSArmouryItemObject
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        [Header("References")]
        public GameObject OutStockObject;
        public TMP_Text PurchaseCostText;
        public Image IconImage;

        protected override void OnAssignedItem()
        {
            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();

            IconImage.sprite = AssignedItem.Icon;

            CheckAvailability();
        }

        void CheckAvailability()
        {
            OutStockObject.SetActive(!AssignedItem.InStock);
        }

        public void OnClick()
        {
            InstantiateUI<BsArmouryItemPopup>(PopupObject).Assign(AssignedItem, () =>
            {
                CheckAvailability();
            });
        }
    }
}
