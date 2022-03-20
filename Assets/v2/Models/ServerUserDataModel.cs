using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Inventory.Models;
using GM.Mercs;
using GM.PlayerStats;
using Newtonsoft.Json;
using GM.Quests;
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
        UserStatsModel UserStats { get; set; }
        QuestsDataResponse Quests { get; }
    }

    public class ServerUserDataModel : IServerUserData
    {
        public List<UserMercDataModel> UnlockedMercs { get; set; }
        public UserCurrenciesModel CurrencyItems { get; set; }
        public CompleteBountyDataModel BountyData { get; set; }
        public List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<ArtefactUserDataModel> Artefacts { get; set; }
        public CompleteBountyShopDataModel BountyShop { get; set; }
        public UserStatsModel UserStats { get; set; }
        public QuestsDataResponse Quests { get; set; }
    }
}
