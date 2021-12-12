namespace GM.HTTP.Requests
{
    public class PurchaseBountyShopItem : IServerRequest
    {
        public string ShopItem { get; set; }

        public PurchaseBountyShopItem(string item)
        {
            ShopItem = item;
        }
    }

    public class PurchaseArmouryItemResponse : ServerResponse
    {
        public long PurchaseCost { get; set; }
        public Inventory.Models.UserCurrenciesModel CurrencyItems { set; get; }
        public Armoury.Models.ArmouryItemUserDataModel ArmouryItem { get; set; }
    }
}