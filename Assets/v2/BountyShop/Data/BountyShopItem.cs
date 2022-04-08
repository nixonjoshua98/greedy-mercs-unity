using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    /// <summary>Base implementation of the interface</summary>
    public class BountyShopItem : Core.GMClass
    {
        public string ID;
        public int PurchaseLimit;
        public int PurchaseCost;

        [JsonIgnore]
        public bool InStock => PurchaseLimit >= App.BountyShop.GetItemPurchaseData(ID);
    }
}
