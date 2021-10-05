namespace GM.HTTP.Requests
{
    public class UpgradeArmouryItemRequest : AuthorisedServerRequest
    {
        public int ItemId;
    }


    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Armoury.Models.UserArmouryItemModel UpdatedItem;
    }
}
