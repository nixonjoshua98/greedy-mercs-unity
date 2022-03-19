using System;

namespace GM.PlayerStats
{

    // Make timed quests and this online only and fetch it once the daily quests tab is shown

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
