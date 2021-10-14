namespace GM.HTTP.Requests
{
    public class UpgradeArmouryItemRequest : AuthenticatedRequest
    {
        public int ItemId;
    }


    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems;
        public Armoury.Models.ArmouryItemUserDataModel UpdatedItem;
    }
}
