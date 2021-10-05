﻿using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpgradeArmouryItemRequest : AuthorisedServerRequest
    {
        public int ItemId;
    }


    public class UpgradeArmouryItemResponse : ServerResponse
    {
        public Inventory.Models.UserCurrenciesModel UserCurrencies;
        public List<Armoury.Models.UserArmouryItemModel> ArmouryItems;
    }
}
