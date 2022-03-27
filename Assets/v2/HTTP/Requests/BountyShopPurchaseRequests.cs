using GM.Inventory;

namespace GM.HTTP.Requests.BountyShop
{
    // = Requests = //

    public class PurchaseBountyShopItem : IServerRequest
    {
        public string ItemID { get; set; }
    }

    // = Responses = //

    public class PurchaseArmouryItemResponse
    {
        public long PurchaseCost { get; set; }
        public UserCurrencies CurrencyItems { set; get; }
        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem { get; set; }
    }

    public class PurchaseCurrencyResponse : ServerResponse
    {
        public long PurchaseCost;
        public UserCurrencies Currencies;
        public int CurrencyGained;
    }
}