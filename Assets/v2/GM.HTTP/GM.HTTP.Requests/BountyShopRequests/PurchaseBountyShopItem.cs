namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : IServerRequest
    {
        public string ShopItem;
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;

        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem;
    }
}
