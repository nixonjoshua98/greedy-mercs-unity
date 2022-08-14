using GM.Common.Enums;
using GM.ScriptableObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace GM.BountyShop.Data
{
    public abstract class BountyShopItem : Core.GMClass
    {
        public string ID;
        public int PurchaseLimit;
        public int PurchaseCost;

        [JsonIgnore]
        public bool InStock => PurchaseLimit >= App.BountyShop.GetItemPurchaseData(ID);
    }

    public class BountyShopCurrencyItem : BountyShopItem
    {
        public CurrencyType CurrencyType;
        public int Quantity;

        [JsonIgnore]
        public Sprite Icon => Item.Icon;

        [JsonIgnore]
        public CurrencyConfig Item => App.Local.GetCurrency(CurrencyType);
    }

    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ItemID;

        [JsonIgnore]
        public Armoury.AggregatedArmouryItem Item => App.Armoury.GetItem(ItemID);
    }

    public class BountyShopPurchaseModel
    {
        public string ItemID;
    }

    public class BountyShopItems
    {
        public List<BountyShopArmouryItem> ArmouryItems;
        public List<BountyShopCurrencyItem> CurrencyItems;
    }
}
