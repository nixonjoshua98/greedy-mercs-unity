using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.BountyModels
{
    public class UpdateActiveBountiesRequest : AuthorisedServerRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public List<int> BountyIds;
    }
}
