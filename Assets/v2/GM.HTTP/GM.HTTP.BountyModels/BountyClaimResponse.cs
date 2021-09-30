using GM.Common.Json;
using Newtonsoft.Json;
using System;

namespace GM.HTTP.BountyModels
{
    public class BountyClaimResponse : ServerResponse
    {
        public int PointsClaimed { get; set; }

        [JsonConverter(typeof(UnixMillisecondDateTimeConverter))]
        public DateTime ClaimTime { get; set; }

        [JsonProperty(PropertyName = "userItems")]
        public GM.Models.UserCurrenciesModel UserCurrencies { get; set; }
    }
}