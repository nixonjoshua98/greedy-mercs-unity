using GM.Common.Enums;
using GM.CurrencyItems.ScriptableObjects;
using Newtonsoft.Json;

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
        public CurrencyItemScriptableObject Item => App.Items.GetItem(CurrencyType);
    }

    public class BountyShopArmouryItem : BountyShopItem
    {
        public int ItemID;

        [JsonIgnore]
        public Armoury.AggregatedArmouryItem Item => App.Armoury.GetItem(ItemID);
    }
}
