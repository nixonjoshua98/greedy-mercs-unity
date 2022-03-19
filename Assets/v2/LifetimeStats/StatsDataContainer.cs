namespace GM.PlayerStats
{
    public class StatsDataContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel Lifetime;

        public void Set(LifetimeStatsModel userData)
        {
            Lifetime = userData;
        }
    }
}
