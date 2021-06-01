﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop
{
    using GM.UI;
    using GM.Armoury;

    public class BsArmouryItemSlot : AbstractBountyShopSlot
    {
        [Header("Components - Scripts")]
        [SerializeField] StarRatingController starController;

        ArmouryItemData ArmouryItemData { get { return StaticData.Armoury.Get(ShopItemData.ArmouryItemID); } }
        new BsArmouryItemData ShopItemData { get { return BountyShopManager.Instance.ServerData.GetArmouryItem(_itemId); } }

        void Awake()
        {
            // WARNING: Do not assume that _itemId has been set here
            // hours_wasted = 1
        }

        protected override void OnItemAssigned()
        {
            base.OnItemAssigned();

            starController.Show(ArmouryItemData.Tier);
        }

        void FixedUpdate()
        {
            if (!_isUpdatingUi)
                return;

            outStockObject.SetActive(!BountyShopManager.Instance.InStock(ShopItemData.ID));

            purchaseCostText.text = ShopItemData.PurchaseCost.ToString();
        }


        // = = = Button Callbacks ===
        public void OnPurchaseButton()
        {
            BountyShopManager.Instance.PurchaseArmouryItem(ShopItemData.ID, (_) => { });
        }
    }
}