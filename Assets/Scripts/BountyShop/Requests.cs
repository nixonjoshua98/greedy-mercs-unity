using GM.Armoury.Data;
using GM.BountyShop.Data;
using GM.Inventory;
using System;
using System.Collections.Generic;

namespace GM.BountyShop.Requests
{
    public class PurchaseArmouryItemResponse : GM.HTTP.Requests.ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
        public UserArmouryItem ArmouryItem;
    }

    public class PurchaseCurrencyResponse : GM.HTTP.Requests.ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
    }

    public class GetBountyShopResponse : GM.HTTP.Requests.ServerResponse
    {
        public DateTime ShopCreationTime;
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItems ShopItems;
    }
}