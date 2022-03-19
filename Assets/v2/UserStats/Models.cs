using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GM.PlayerStats
{
    public class UserStatsModel
    {
        public LifetimeStatsModel Lifetime;
        public DailyStatsModel Daily;
    }

    public class LifetimeStatsModel
    {
        public int TotalPrestiges;
        public int HighestPrestigeStageReached;
    }

    public class DailyStatsModel
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(PropertyName = "fromDate")]
        public DateTime LastRefresh { get; set; }

        public int TotalPrestiges = 0;
    }
}
