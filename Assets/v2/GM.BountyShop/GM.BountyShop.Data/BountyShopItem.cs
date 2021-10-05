using Newtonsoft.Json;

namespace GM.BountyShop.Data
{
    /// <summary>
    /// Interface for each bounty item
    /// </summary>
    public interface IBountyShopItem
    {
        string Id { get; set; }
        int DailyPurchaseLimit { get; set; }
        int PurchaseCost { get; set; }
        public bool InStock { get; }
    }

    /// <summary>
    /// Base implementation of the interface
    /// </summary>
    public class BountyShopItem : Core.GMClass, IBountyShopItem
    {
        [JsonProperty(PropertyName = "itemId")]
        public string Id { get; set; }
        public int DailyPurchaseLimit { get; set; }
        public int PurchaseCost { get; set; }

        public bool InStock
        {
            get
            {
                var purchase = App.Data.BountyShop.GetItemPurchaseData(Id);

                return DailyPurchaseLimit > purchase.TotalDailyPurchases;
            }
        }
    }
}
