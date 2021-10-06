using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : AuthorisedServerRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "userItems")]
        public Inventory.Models.UserCurrenciesModel UserCurrencies;

        public Dictionary<string, int> DailyPurchases;

        [JsonProperty(PropertyName = "userArmouryItems")]
        public Dictionary<int, Armoury.Models.UserArmouryItemModel> ArmouryItems;
    }
}
