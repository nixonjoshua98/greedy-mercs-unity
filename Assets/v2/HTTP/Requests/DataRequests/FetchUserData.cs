using System.Collections.Generic;
using Newtonsoft.Json;

namespace GM.HTTP.Requests
{
    public class FetchUserDataResponse : ServerResponse, Common.Data.IServerUserData
    {
        [JsonProperty(PropertyName = "unlockedUnits")]
        public List<GM.Mercs.Models.UserMercDataModel> UnlockedMercs { get; set; }

        public Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
        public Bounties.Models.CompleteBountyDataModel BountyData { get; set; }
        public List<Armoury.Models.ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<Artefacts.Models.ArtefactUserDataModel> Artefacts { get; set; }
        public GM.BountyShop.Models.CompleteBountyShopDataModel BountyShop { get; set; }
    }
}
