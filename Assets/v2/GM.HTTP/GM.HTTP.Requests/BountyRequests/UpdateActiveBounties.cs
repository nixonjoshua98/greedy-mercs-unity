using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpdateActiveBountiesRequest : AuthorisedServerRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public List<int> BountyIds; // List of target active bounties
    }

    public class UpdateActiveBountiesResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "bounties")]
        public List<Bounties.Models.SingleBountyUserDataModel> Bounties;
    }
}
