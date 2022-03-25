namespace GM.HTTP.Requests
{
    public class UpgradeArmouryItemRequest : IServerRequest
    {
        public readonly int ItemId;

        public UpgradeArmouryItemRequest(int item)
        {
            ItemId = item;
        }
    }


    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrencies CurrencyItems;
        public Armoury.Models.ArmouryItemUserDataModel Item;

        public int UpgradeCost;
    }
}
