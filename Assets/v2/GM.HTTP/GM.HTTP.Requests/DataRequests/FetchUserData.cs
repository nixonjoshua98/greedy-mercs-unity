using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class FetchUserDataRequest : AuthenticatedRequest
    {

    }

    public class FetchUserDataResponse : ServerResponse, Common.IServerUserData
    {
        public Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
        public Bounties.Models.CompleteBountyDataModel BountyData { get; set; }
        public List<Armoury.Models.ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<Artefacts.Models.ArtefactUserDataModel> Artefacts { get; set; }
        public BountyShop.Models.CompleteBountyShopDataModel BountyShop { get; set; }
    }
}
