using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class EvolveArmouryItemRequest : AuthenticatedRequest
    {
        public int ItemId;
    }


    public class EvolveArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.ArmouryItemUserDataModel UpdatedItem;
    }
}
