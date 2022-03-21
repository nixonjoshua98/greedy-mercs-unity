﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GM.PlayerStats
{
    public class UserStatsResponse : GM.HTTP.ServerResponse
    {
        public LifetimeStatsModel Lifetime;
        public DailyStatsModel Daily;
    }


    public class PlayerStats
    {
        public int TotalPrestiges;
        public int TotalEnemiesDefeated;
        public int TotalBossesDefeated;
        public int HighestPrestigeStageReached;
    }


    public class LifetimeStatsModel : PlayerStats
    {

    }


    public class DailyStatsModel : PlayerStats
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextRefresh { get; set; }
    }
}
