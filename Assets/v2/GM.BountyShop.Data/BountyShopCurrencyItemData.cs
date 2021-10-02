using UnityEngine;

namespace GM.BountyShop.Data
{
    public class BountyShopCurrencyItemData : BountyShopItem
    {
        Items.Data.FullGameItemData Item => App.Data.Items.GetItem(ItemType);

        public Items.Data.ItemType ItemType;
        public int QuantityPerPurchase;

        public Sprite Icon => Item.Icon;
        public string DisplayName => Item.DisplayName;
    }
}