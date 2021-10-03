using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.BountyShopModels
{
    public class PurchaseBountyShopItemRequest : AuthorisedServerRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "userItems")]
        public GM.Inventory.Data.UserCurrencies UserCurrencies;

        public Dictionary<string, int> DailyPurchases;

        [JsonProperty(PropertyName = "userArmouryItems")]
        public Dictionary<int, GM.Armoury.Data.ArmouryItemState> ArmouryItems;
    }
}
