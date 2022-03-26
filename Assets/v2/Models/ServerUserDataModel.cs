using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Inventory;
using GM.Mercs;
using GM.PlayerStats;
using GM.Quests;
using System.Collections.Generic;

namespace GM.Models
{
    public interface IServerUserData
    {
        List<UserMercDataModel> UnlockedMercs { get; set; }
        UserCurrencies Currencies { get; set; }
        UserBounties Bounties { get; set; }
        List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        List<ArtefactUserDataModel> Artefacts { get; set; }
        CompleteBountyShopDataModel BountyShop { get; set; }
        PlayerStatsResponse UserStats { get; set; }
        QuestsDataResponse Quests { get; }
    }

    public class ServerUserDataModel : IServerUserData
    {
        public List<UserMercDataModel> UnlockedMercs { get; set; }
        public UserCurrencies Currencies { get; set; }
        public UserBounties Bounties { get; set; }
        public List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<ArtefactUserDataModel> Artefacts { get; set; }
        public CompleteBountyShopDataModel BountyShop { get; set; }
        public PlayerStatsResponse UserStats { get; set; }
        public QuestsDataResponse Quests { get; set; }
    }
}
