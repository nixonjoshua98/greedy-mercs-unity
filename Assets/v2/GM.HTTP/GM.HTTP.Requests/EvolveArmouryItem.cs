using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class EvolveArmouryItemRequest : AuthorisedServerRequest
    {
        public int ItemId;
    }


    public class EvolveArmouryItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel UserCurrencies;
        public List<Armoury.Models.UserArmouryItemModel> ArmouryItems;
    }
}
