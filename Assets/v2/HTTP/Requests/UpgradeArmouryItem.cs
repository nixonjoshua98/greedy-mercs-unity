namespace GM.HTTP.Requests
{
    public class UpgradeArmouryItemRequest : IServerRequest
    {
        public int ItemID;
    }


    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.ArmouryItemUserData Item;

        public int UpgradeCost;
    }
}
