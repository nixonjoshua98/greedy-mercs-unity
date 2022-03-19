namespace GM.PlayerStats
{
    public class StatsDataContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel Lifetime;
        public DailyStatsModel Daily;

        public void Set(UserStatsModel userData)
        {
            Lifetime = userData.Lifetime;
            Daily = userData.Daily;
        }
    }
}
