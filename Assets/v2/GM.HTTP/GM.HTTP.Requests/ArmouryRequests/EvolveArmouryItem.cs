namespace GM.HTTP.Requests
{
    public class UpgradeStarLevelArmouryItemRequest : IServerRequest
    {
        public int ItemId;
    }


    public class UpgradeStarLevelArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.ArmouryItemUserDataModel UpdatedItem;
    }
}
