﻿using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Inventory.Models;
using GM.Mercs;
using System.Collections.Generic;

namespace GM.Models
{
    public interface IServerUserData
    {
        List<UserMercDataModel> UnlockedMercs { get; set; }
        UserCurrenciesModel CurrencyItems { get; set; }
        CompleteBountyDataModel BountyData { get; set; }
        List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        List<ArtefactUserDataModel> Artefacts { get; set; }
        CompleteBountyShopDataModel BountyShop { get; set; }
        Quests.UserQuestsModel Quests { get; set; }
    }

    public class ServerUserDataModel : IServerUserData
    {
        public List<UserMercDataModel> UnlockedMercs { get; set; }
        public UserCurrenciesModel CurrencyItems { get; set; }
        public CompleteBountyDataModel BountyData { get; set; }
        public List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<ArtefactUserDataModel> Artefacts { get; set; }
        public CompleteBountyShopDataModel BountyShop { get; set; }
        public Quests.UserQuestsModel Quests { get; set; }
    }
}
