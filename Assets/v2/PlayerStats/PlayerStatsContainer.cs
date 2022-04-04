using GM.HTTP;
using System;

namespace GM.PlayerStats
{
    public class PlayerStatsContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel ConfirmedLifetimeStats;
        public LifetimeStatsModel LocalLifetimeStats { get => App.PersistantLocalFile.LocalLifetimeStats; set => App.PersistantLocalFile.LocalLifetimeStats = value; }

        public TimedPlayerStatsModel LocalDailyStats => App.PersistantLocalFile.LocalDailyStats;

        public int HighestStageReached => Math.Max(App.GameState.Stage, ConfirmedLifetimeStats.HighestPrestigeStage);

        public void Set(LifetimeStatsModel userData)
        {
            ConfirmedLifetimeStats = userData;
        }

        public void UpdateLifetimeStats(Action<bool> action)
        {
            App.HTTP.UpdateLifetimeStats(resp =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    ConfirmedLifetimeStats = resp.LifetimeStats;

                    LocalLifetimeStats = new();
                }

                action.Invoke(resp.StatusCode == HTTPCodes.Success);
            });
        }
    }
}
