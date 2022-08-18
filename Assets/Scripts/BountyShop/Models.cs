using Newtonsoft.Json;
using SRC.Common.Enums;
using SRC.ScriptableObjects;
using System.Collections.Generic;

namespace SRC.BountyShop.Data
{
    public abstract class BountyShopItem : Core.GMClass
    {
        public string ID;
        public int PurchaseCost;

        [JsonIgnore]
        public abstract bool InStock { get; }
    }

    public class BountyShopCurrencyItem : BountyShopItem
    {
        public CurrencyType CurrencyType;
        public int Quantity;

        [JsonIgnore]
        public CurrencyConfig Item => App.Local.GetCurrency(CurrencyType);

        [JsonIgnore]
        public override bool InStock => App.BountyShop.GetItemPurchase(BountyShopItemType.CurrencyItem, ID) is null;
    }

    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ItemID;

        public override bool InStock => App.BountyShop.GetItemPurchase(BountyShopItemType.ArmouryItem, ID) is null;

        [JsonIgnore]
        public Armoury.AggregatedArmouryItem Item => App.Armoury.GetItem(ItemID);
    }

    public class BountyShopPurchaseModel
    {
        public string ItemID;
        public BountyShopItemType ItemType;
    }

    public class BountyShopItems
    {
        public List<BountyShopArmouryItem> ArmouryItems;
        public List<BountyShopCurrencyItem> CurrencyItems;
    }
}
