using System;

namespace GM.PlayerStats
{
    public class StatsDataContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel Lifetime;
        public DailyStatsModel Daily { get; set; }

        public bool IsDailyStatsValid => DateTime.UtcNow < Daily.NextRefresh;

        public int HighestStageReached { get => Math.Max(App.GameState.Stage, Lifetime.HighestPrestigeStageReached); }

        public void Set(UserStatsModel userData)
        {
            Lifetime = userData.Lifetime;
            Daily = userData.Daily;
        }

        public void FetchDailyStats(Action<bool> action)
        {
            App.HTTP.FetchDailyStats(resp =>
            {
                if (resp.StatusCode == GM.HTTP.HTTPCodes.Success)
                {
                    Daily = resp;
                }

                action.Invoke(resp.StatusCode == GM.HTTP.HTTPCodes.Success);
            });
        }
    }
}
