using System;

namespace GM.PlayerStats
{
    public class StatsDataContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel Lifetime;
        public DailyStatsModel Daily;

        public int HighestStageReached { get => Math.Max(App.GameState.Stage, Lifetime.HighestPrestigeStageReached); }

        public void Set(UserStatsModel userData)
        {
            Lifetime = userData.Lifetime;
            Daily = userData.Daily;
        }
    }
}
