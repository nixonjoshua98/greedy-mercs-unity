using GM.Armoury.Data;
using GM.BountyShop.Data;
using GM.HTTP;
using GM.Inventory;
using System;
using System.Collections.Generic;

namespace GM.BountyShop.Requests
{
    // = Requests = //

    public class PurchaseBountyShopItem : IServerRequest
    {
        public string ItemID { get; set; }
    }

    // = Responses = //

    public class PurchaseArmouryItemResponse : ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
        public UserArmouryItem ArmouryItem;
    }

    public class PurchaseCurrencyResponse : ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
    }

    // = Get Bounty Shop = //

    public class GetBountyShopResponse : ServerResponse
    {
        public DateTime ShopCreationTime;
        public List<BountyShopPurchaseModel> Purchases;
        public BountyShopItems ShopItems;
    }
}