using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : AuthorisedRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;

        public Armoury.Models.UserArmouryItemModel ArmouryItem;
    }
}
