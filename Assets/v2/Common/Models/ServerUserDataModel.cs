using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Common.Interfaces;
using GM.Inventory.Models;
using GM.Mercs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.Common.Models
{
    public class ServerUserDataModel : IServerUserData
    {
        [JsonProperty(PropertyName = "unlockedUnits")]
        public List<UserMercDataModel> UnlockedMercs { get; set; }

        public UserCurrenciesModel CurrencyItems { get; set; }
        public CompleteBountyDataModel BountyData { get; set; }
        public List<ArmouryItemUserDataModel> ArmouryItems { get; set; }
        public List<ArtefactUserDataModel> Artefacts { get; set; }
        public CompleteBountyShopDataModel BountyShop { get; set; }
    }
}
