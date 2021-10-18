using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpdateActiveBountiesRequest : AuthenticatedRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public List<int> BountyIds; // List of target active bounties
    }

    public class UpdateActiveBountiesResponse : ServerResponse
    {
        public List<Bounties.Models.SingleBountyUserDataModel> Bounties;
    }
}
