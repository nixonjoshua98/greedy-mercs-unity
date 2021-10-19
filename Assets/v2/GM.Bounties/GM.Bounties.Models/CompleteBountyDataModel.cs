using GM.Common.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GM.Bounties.Models
{
    public class CompleteBountyDataModel
    {
        [JsonConverter(typeof(UnixMillisecondUTCDateTimeConverter))]
        public DateTime LastClaimTime;

        public List<BountyUserDataModel> Bounties;
    }
}
