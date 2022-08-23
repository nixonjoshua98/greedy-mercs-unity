using SRC.BountyShop.Data;
using SRC.BountyShop.UI.BaseClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRC.BountyShop.UI.ItemSlots
{
    public class BountyShopArmouryItemSlot : BountyShopItemSlot<BountyShopArmouryItem>
    {
        public override void BuyItem()
        {
            App.BountyShop.PurchaseArmouryItem(ShopItem.ID, resp =>
            {
                UpdateSlotUI();
            });
        }

        protected override void UpdateUI()
        {
            Icon.Initialize(ShopItem.Item);

            NameText.text = $"<color=orange>[Armoury]</color> {ShopItem.Item.Name}";
        }
    }
}
