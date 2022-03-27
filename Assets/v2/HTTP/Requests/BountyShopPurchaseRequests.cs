using GM.Inventory;

namespace GM.HTTP.Requests.BountyShop
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
        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem;
    }

    public class PurchaseCurrencyResponse : ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
        public int CurrencyGained;
    }
}