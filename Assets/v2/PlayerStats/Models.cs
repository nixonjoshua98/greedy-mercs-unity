﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GM.PlayerStats
{
    public class PlayerStatsResponse : GM.HTTP.ServerResponse
    {
        public LifetimeStatsModel Lifetime;
        public TimedStatsModel Daily;
    }


    public class PlayerStats
    {
        public int TotalPrestiges;
        public int TotalEnemiesDefeated;
        public int TotalTaps;
        public int TotalBossesDefeated;
        public int HighestPrestigeStage;
    }


    public class LifetimeStatsModel : PlayerStats
    {

    }


    public class TimedStatsModel : PlayerStats
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedTime { get; set; }
    }
}