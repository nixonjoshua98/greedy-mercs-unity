using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using GM.Data;

    public abstract class AbstractBountyShopSlot : MonoBehaviour
    {
        [Header("Children")]
        [SerializeField] protected GameObject outStockObject;

        [Header("Components - UI")]
        [SerializeField] Image iconImage;
        [Space]
        [SerializeField] protected Text purchaseCostText;

        protected string _itemId;
        protected bool _isUpdatingUi = false;

        protected BountyShopItem ShopItemData => UserData.Get.BountyShop.ServerData.GetItem(_itemId);

        public void Setup(string itemId)
        {

            _itemId = itemId;
            _isUpdatingUi = true;

            OnItemAssigned();
        }

        protected virtual void OnItemAssigned()
        {
            iconImage.sprite = ShopItemData.Icon;
        }
    }
}