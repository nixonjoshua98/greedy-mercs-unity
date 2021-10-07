using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class FetchUserDataRequest : AuthorisedRequest
    {

    }

    public class FetchUserDataResponse : ServerResponse, Common.ICompleteUserData
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
        public Bounties.Models.CompleteBountyDataModel BountyData { get; set; }
        public List<Armoury.Models.UserArmouryItemModel> ArmouryItems { get; set; }
        public List<Artefacts.Models.UserArtefactModel> Artefacts { get; set; }
        public BountyShop.Models.CompleteBountyShopDataModel BountyShop { get; set; }
    }
}
