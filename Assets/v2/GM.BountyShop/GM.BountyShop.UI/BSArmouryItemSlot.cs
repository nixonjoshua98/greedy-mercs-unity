using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BountyShopArmouryItem = GM.BountyShop.Data.BountyShopArmouryItem;

namespace GM.BountyShop.UI
{
    public class BSArmouryItemSlot : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject PopupObject;
        [Space]
        public TMP_Text TierText;
        public TMP_Text PurchaseCostText;
        [Space]
        public Image IconImage;

        BountyShopArmouryItem AssignedItem;

        public void AssignShopItem(BountyShopArmouryItem item)
        {
            AssignedItem = item;

            TierText.color = AssignedItem.ItemData.Config.Colour;
            TierText.text = AssignedItem.ItemData.Config.DisplayText;

            PurchaseCostText.text = AssignedItem.PurchaseCost.ToString();

            IconImage.sprite = AssignedItem.Icon;
        }
    }
}
