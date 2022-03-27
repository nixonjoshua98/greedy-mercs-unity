﻿using System;
using GM.HTTP;

namespace GM.PlayerStats
{
    public class PlayerStatsContainer : GM.Core.GMClass
    {
        public LifetimeStatsModel ConfirmedLifetimeStats;
        public LifetimeStatsModel LocalLifetimeStats { get => App.PersistantLocalFile.LocalLifetimeStats; set => App.PersistantLocalFile.LocalLifetimeStats = value; }

        public TimedPlayerStatsModel ConfirmedDailyStats;
        public TimedPlayerStatsModel LocalDailyStats { get => App.PersistantLocalFile.LocalDailyStats; }

        public bool IsDailyStatsValid { get => ConfirmedDailyStats.DateTime.IsBetween(App.DailyRefresh.PreviousNextReset); }
        public int HighestStageReached { get => Math.Max(App.GameState.Stage, ConfirmedLifetimeStats.HighestPrestigeStage); }

        public void Set(PlayerStatsResponse userData)
        {
            ConfirmedLifetimeStats = userData.LifetimeStats;
            ConfirmedDailyStats = userData.DailyStats;
        }

        public void FetchStats(Action<bool> action)
        {
            App.HTTP.FetchStats(resp =>
            {
                if (resp.StatusCode == GM.HTTP.HTTPCodes.Success)
                {
                    ConfirmedDailyStats = resp.DailyStats;
                    ConfirmedLifetimeStats = resp.LifetimeStats;
                }

                action.Invoke(resp.StatusCode == GM.HTTP.HTTPCodes.Success);
            });
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
