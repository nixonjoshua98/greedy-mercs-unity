namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : IServerRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        public long PurchaseCost;

        public Inventory.Models.UserCurrenciesModel CurrencyItems;

        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem;
    }
}
