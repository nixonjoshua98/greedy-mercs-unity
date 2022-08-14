using GM.Armoury.Data;
using GM.Artefacts.Data;
using GM.Bounties.Models;
using GM.Inventory;
using GM.Mercs;
using GM.UserStats;
using GM.Quests;
using System.Collections.Generic;
using GM.Mercs.Data;

namespace GM.Models
{
    public interface IServerUserData
    {
        List<MercUserData> UnlockedMercs { get; set; }
        UserCurrencies Currencies { get; set; }
        UserBounties Bounties { get; set; }
        List<UserArmouryItem> ArmouryItems { get; set; }
        List<ArtefactUserData> Artefacts { get; set; }
        LifetimeStatsModel LifetimeStats { get; set; }
        QuestsDataResponse Quests { get; }
    }

    public class ServerUserDataModel : IServerUserData
    {
        public List<MercUserData> UnlockedMercs { get; set; }
        public UserCurrencies Currencies { get; set; }
        public UserBounties Bounties { get; set; }
        public List<UserArmouryItem> ArmouryItems { get; set; }
        public List<ArtefactUserData> Artefacts { get; set; }
        public LifetimeStatsModel LifetimeStats { get; set; }
        public QuestsDataResponse Quests { get; set; }
    }
}
