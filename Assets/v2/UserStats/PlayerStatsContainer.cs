using System;

namespace GM.PlayerStats
{
    public class PlayerStatsContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel ConfirmedLifetimeStats;
        public DailyStatsModel ConfirmedDailyStats;

        public DailyStatsModel LocalDailyStats { get => App.PersistantLocalFile.LocalDailyStats; }

        public bool IsDailyStatsValid => DateTime.UtcNow < ConfirmedDailyStats.NextRefresh;

        public int HighestStageReached { get => Math.Max(App.GameState.Stage, ConfirmedLifetimeStats.HighestPrestigeStageReached); }

        public void Set(UserStatsResponse userData)
        {
            ConfirmedLifetimeStats = userData.Lifetime;
            ConfirmedDailyStats = userData.Daily;
        }

        public void FetchStats(Action<bool> action)
        {
            App.HTTP.FetchStats(resp =>
            {
                if (resp.StatusCode == GM.HTTP.HTTPCodes.Success)
                {
                    ConfirmedDailyStats = resp.Daily;
                    ConfirmedLifetimeStats = resp.Lifetime;
                }

                action.Invoke(resp.StatusCode == GM.HTTP.HTTPCodes.Success);
            });
        }
    }
}
