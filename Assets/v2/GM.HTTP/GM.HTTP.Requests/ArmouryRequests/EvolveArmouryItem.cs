using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class EvolveArmouryItemRequest : AuthorisedRequest
    {
        public int ItemId;
    }


    public class EvolveArmouryItemResponse : ServerResponse
    {
        public Armoury.Models.UserArmouryItemModel UpdatedItem;
    }
}
