namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItemRequest : IServerRequest
    {
        public readonly string ShopItem;

        public PurchaseBountyShopItemRequest(string item)
        {
            ShopItem = item;
        }
    }

    public class PurchaseBountyShopItemResponse : ServerResponse
    {
        public long PurchaseCost;

        public Inventory.Models.UserCurrenciesModel CurrencyItems;

        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem;
    }
}
