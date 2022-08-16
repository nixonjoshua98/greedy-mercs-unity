using SRC.HTTP;
using System;

namespace SRC.UserStats
{
    public class PlayerStatsContainer : SRC.Core.GMClass
    {
        public LifetimeStatsModel ConfirmedLifetimeStats;

        // I don't like accessing the persistant file directly so we proxy the value through this property
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
