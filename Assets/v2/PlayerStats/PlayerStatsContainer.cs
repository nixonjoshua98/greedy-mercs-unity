using System;

namespace GM.PlayerStats
{
    public class PlayerStatsContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel ConfirmedLifetimeStats;
        public DailyStatsModel ConfirmedDailyStats;

        public DailyStatsModel LocalDailyStats { get => App.PersistantLocalFile.LocalDailyStats; }

        public bool IsDailyStatsValid { get => ConfirmedDailyStats.CreatedTime.IsBetween(App.DailyRefresh.PreviousNextReset); }
        public int HighestStageReached { get => Math.Max(App.GameState.Stage, ConfirmedLifetimeStats.HighestPrestigeStage); }

        public void Set(PlayerStatsResponse userData)
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
