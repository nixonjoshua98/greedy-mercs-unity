using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.BountyModels
{
    public class UpdateActiveBountiesResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "userBounties")]
        public List<GM.Bounties.Data.BountyUserData> Bounties;
    }
}