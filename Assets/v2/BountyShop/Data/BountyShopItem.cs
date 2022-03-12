using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    /// <summary>Interface for each bounty item</summary>
    public interface IBountyShopItem
    {
        string Id { get; set; }
        int purchaseLimit { get; set; }
        int PurchaseCost { get; set; }
        public bool InStock { get; }
    }

    /// <summary>Base implementation of the interface</summary>
    public class BountyShopItem : Core.GMClass, IBountyShopItem
    {
        [JsonProperty(PropertyName = "itemId", Required = Required.Always)]
        public string Id { get; set; }
        public int purchaseLimit { get; set; }
        public int PurchaseCost { get; set; }

        [JsonIgnore]
        public bool InStock => purchaseLimit > App.DataContainers.BountyShop.GetItemPurchaseData(Id);
    }
}
