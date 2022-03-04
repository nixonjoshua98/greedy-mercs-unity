using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Inventory.Models;
using GM.Mercs;
using System.Collections.Generic;

namespace GM.Common.Interfaces
{
    public interface IServerUserData
    {
        List<UserMercDataModel> UnlockedMercs { get; set; }
        UserCurrenciesModel CurrencyItems { get; set; }
        CompleteBountyDataModel BountyData { get; set; }
        List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        List<ArtefactUserDataModel> Artefacts { get; set; }
        CompleteBountyShopDataModel BountyShop { get; set; }
    }
}