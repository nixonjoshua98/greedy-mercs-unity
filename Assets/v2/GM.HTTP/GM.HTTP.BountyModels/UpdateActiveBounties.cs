using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.BountyModels
{
    public class UpdateActiveBountiesRequest : AuthorisedServerRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public List<int> BountyIds; // List of target active bounties
    }

    public class UpdateActiveBountiesResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "bounties")]
        public List<GM.Bounties.Data.BountyUserData> Bounties;
    }
}
