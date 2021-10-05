using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopCurrencyItemData : BountyShopItem
    {
        public CurrencyItems.Data.CurrencyType ItemType;
        public int QuantityPerPurchase;

        public Sprite Icon => ItemData.Icon;
        public string DisplayName => ItemData.DisplayName;

        CurrencyItems.Data.CurrencyItemData ItemData => App.Data.Items.GetItem(ItemType);
    }
}