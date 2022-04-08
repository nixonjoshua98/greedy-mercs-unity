using System;

namespace GM.PlayerStats
{
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


    public class TimedPlayerStatsModel : PlayerStats
    {
        public DateTime DateTime;

    }
}
