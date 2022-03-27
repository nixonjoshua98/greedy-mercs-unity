using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    /// <summary>Interface for each bounty item</summary>
    public interface IBountyShopItem
    {
        string ID { get; set; }
        int PurchaseLimit { get; set; }
        int PurchaseCost { get; set; }
        public bool InStock { get; }
    }

    /// <summary>Base implementation of the interface</summary>
    public class BountyShopItem : Core.GMClass, IBountyShopItem
    {
        public string ID { get; set; }
        public int PurchaseLimit { get; set; } = 1;
        public int PurchaseCost { get; set; }

        [JsonIgnore]
        public bool InStock => PurchaseLimit > App.BountyShop.GetItemPurchaseData(ID);
    }
}
