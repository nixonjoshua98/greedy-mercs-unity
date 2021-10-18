using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : AuthenticatedRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;

        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem;
    }
}
