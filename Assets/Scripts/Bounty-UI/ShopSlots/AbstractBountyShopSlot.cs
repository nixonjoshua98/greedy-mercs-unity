﻿using System.Collections;
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

        protected int _itemId;
        protected bool _isUpdatingUi = false;

        protected AbstractBountyShopData ShopItemData { get { return BountyShopManager.Instance.ServerData.Get(_itemId); } }

        public virtual void SetID(int itemId)
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