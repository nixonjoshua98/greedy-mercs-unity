using Newtonsoft.Json;
using System.Collections.Generic;

namespace GM.HTTP.Models
{
    public class UpdateActiveBountiesRequest : AuthorisedServerRequest
    {
        [JsonProperty(PropertyName = "bountyIds")]
        public List<int> BountyIds;
    }
}
