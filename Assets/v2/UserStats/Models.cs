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

    public class DailyStatsModel : GM.HTTP.ServerResponse
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextRefresh { get; set; }

        public int TotalPrestiges = 0;
    }
}
