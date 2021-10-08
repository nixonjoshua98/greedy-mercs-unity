using GM.Common.Json;
using Newtonsoft.Json;
using System;

namespace GM.HTTP.Requests
{
    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed { get; set; }

        [JsonConverter(typeof(UnixMillisecondUTCDateTimeConverter))]
        public DateTime ClaimTime { get; set; } // Server time when claimed may be slightly different from the client time

        public Inventory.Models.UserCurrenciesModel CurrencyItems { get; set; }
    }
}