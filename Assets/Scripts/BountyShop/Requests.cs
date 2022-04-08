using GM.Armoury.Data;
using GM.HTTP;
using GM.Inventory;

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
        public int CurrencyGained;
    }
}