using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SRC.Bounties.Shop.UI
{
    public class BountyShopPanel : SRC.UI.PopupBase
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;

        [Header("Transforms")]
        [SerializeField] Transform ShopItemParent;

        private void Awake()
        {
            App.BountyShop.FetchShop(InstantiateShopItems);
        }

        void InstantiateShopItems()
        {
           foreach (var aItem in App.BountyShop.ArmouryItems)
            {
                var slot = this.Instantiate<BountyShopListSlot>(ArmouryItemObject, ShopItemParent);

                slot.Initialize(aItem);
            }

            ShowInnerPanel();
        }
    }
}
