using System.Collections.Generic;

namespace GM.Common
{
    public interface IServerUserData
    {
        Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
        Bounties.Models.CompleteBountyDataModel BountyData { get; set; }
        List<Armoury.Models.ArmouryItemUserDataModel> ArmouryItems { get; set; }
        List<Artefacts.Models.ArtefactUserDataModel> Artefacts { get; set; }
        BountyShop.Models.CompleteBountyShopDataModel BountyShop { get; set; }
    }

}
