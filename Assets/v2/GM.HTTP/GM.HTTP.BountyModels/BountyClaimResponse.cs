using GM.Common.Json;
using Newtonsoft.Json;
using System;

namespace GM.HTTP.BountyModels
{
    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed { get; set; }

        [JsonConverter(typeof(UnixMillisecondDateTimeConverter))]
        public DateTime ClaimTime { get; set; } // Server time when claimed ,ay be slightly different from the client time

        [JsonProperty(PropertyName = "userItems")]
        public GM.Inventory.Data.UserCurrencies UserCurrencies { get; set; }
    }
}