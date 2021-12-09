using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace GM.Bounties.Models
{
    public class CompleteBountyDataModel
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LastClaimTime;

        public List<BountyUserDataModel> Bounties;
    }
}
