using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpgradeStarLevelArmouryItemRequest : AuthenticatedRequest
    {
        public int ItemId;
    }


    public class UpgradeStarLevelArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.ArmouryItemUserDataModel UpdatedItem;
    }
}
