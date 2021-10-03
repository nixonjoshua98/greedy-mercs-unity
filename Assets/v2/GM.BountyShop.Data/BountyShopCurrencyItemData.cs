using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopCurrencyItemData : BountyShopItem
    {
        public Items.Data.ItemType ItemType;
        public int QuantityPerPurchase;

        public Sprite Icon => ItemData.Icon;
        public string DisplayName => ItemData.DisplayName;

        Items.Data.FullGameItemData ItemData => App.Data.Items.GetItem(ItemType);
    }
}