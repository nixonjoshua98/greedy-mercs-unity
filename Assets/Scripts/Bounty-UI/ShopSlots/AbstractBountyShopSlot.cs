using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    public abstract class AbstractBountyShopSlot : MonoBehaviour
    {
        [Header("Children")]
        [SerializeField] protected GameObject outStockObject;

        [Header("Components - UI")]
        [SerializeField] Image iconImage;
        [Space]
        [SerializeField] protected Text purchaseCostText;

        int _itemId;
        protected bool _isUpdatingUi = false;

        protected AbstractBountyShopData ServerItemData { get { return BountyShopManager.Instance.ServerData.Get(_itemId); } }

        public void SetID(int itemId)
        {
            _itemId = itemId;
            _isUpdatingUi = true;

            iconImage.sprite = ServerItemData.Icon;
        }
    }
}