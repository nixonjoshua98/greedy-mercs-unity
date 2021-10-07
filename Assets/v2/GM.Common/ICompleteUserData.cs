using System.Collections.Generic;

namespace GM.Common
{
    public interface ICompleteUserData
    {
        Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
        Bounties.Models.CompleteBountyDataModel BountyData { get; set; }
        List<Armoury.Models.UserArmouryItemModel> ArmouryItems { get; set; }
        List<Artefacts.Models.UserArtefactModel> Artefacts { get; set; }
        BountyShop.Models.CompleteBountyShopDataModel BountyShop { get; set; }
    }

}
