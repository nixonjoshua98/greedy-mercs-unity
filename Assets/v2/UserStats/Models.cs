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
        public int TotalPrestiges;
    }
}
