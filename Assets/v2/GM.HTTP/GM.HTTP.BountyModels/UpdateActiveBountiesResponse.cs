using Newtonsoft.Json;
using System.Collections.Generic;

using UserBountyState = GM.Bounties.Data.UserBountyState;

namespace GM.HTTP.BountyModels
{
    public class UpdateActiveBountiesResponse : ServerResponse
    {
        [JsonProperty(PropertyName = "userBounties")]
        public List<UserBountyState> Bounties;
    }
}