using SRC.Armoury.Data;
using SRC.BountyShop.Data;
using SRC.Inventory;
using System;
using System.Collections.Generic;

namespace SRC.BountyShop.Requests
{
    public class PurchaseArmouryItemResponse : SRC.HTTP.Requests.ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
        public UserArmouryItem ArmouryItem;
    }

    public class PurchaseCurrencyResponse : SRC.HTTP.Requests.ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
    }

    public class GetBountyShopResponse : SRC.HTTP.Requests.ServerResponse
    {
        public DateTime ShopCreationTime;
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItems ShopItems;
    }
}