using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Requests
{
    public class UpdateActiveBountiesRequest : IServerRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public readonly List<int> BountyIds;

        public UpdateActiveBountiesRequest(List<int> bounties)
        {
            BountyIds = bounties;
        }
    }

    public class UpdateActiveBountiesResponse : ServerResponse
    {
        public List<Bounties.Models.BountyUserDataModel> Bounties;
    }
}
