using System;

namespace SRC.UserStats
{
    public class UserAccountStats
    {
        public int TotalPrestiges;
        public int TotalEnemiesDefeated;
        public int TotalTaps;
        public int TotalBossesDefeated;
        public int HighestPrestigeStage;
    }


    public class LifetimeStatsModel : UserAccountStats
    {

    }


    public class TimedPlayerStatsModel : UserAccountStats
    {
        public DateTime LastUpdated;

    }
}
