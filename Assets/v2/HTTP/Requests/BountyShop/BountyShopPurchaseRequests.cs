namespace GM.HTTP.Requests
{
    // = Requests = //

    public class PurchaseBountyShopItem : IServerRequest
    {
        public string ItemId { get; set; }

        public PurchaseBountyShopItem(string item)
        {
            ItemId = item;
        }
    }

    // = Responses = //

    public class BountyShopPurchaseResponse: ServerResponse
    {
        public long PurchaseCost { get; set; }
        public Inventory.Models.UserCurrenciesModel CurrencyItems { set; get; }
    }

    public class PurchaseArmouryItemResponse : BountyShopPurchaseResponse
    {
        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem { get; set; }
    }

    public class PurchaseCurrencyTypeResponse : BountyShopPurchaseResponse
    {

    }
}