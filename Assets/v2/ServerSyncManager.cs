using System.Collections;
using UnityEngine;
using System.Diagnostics;

namespace GM
{
    public class ServerSyncManager : GM.Core.GMMonoBehaviour
    {
        const float StatsSyncInterval = 1_000 * 30; // 30 Seconds

        bool isUpdatingQuests;
        bool isFetchingDailyStats;
        bool isUpdatingLifetimeStats;

        Stopwatch dailyStatsSyncStopwatch;

        void Awake()
        {
            dailyStatsSyncStopwatch = Stopwatch.StartNew();
        }

        void Start()
        {
            StartCoroutine(_UpdateLoop());
        }

        IEnumerator _UpdateLoop()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);

                if (!isUpdatingQuests && !App.Quests.IsDailyQuestsValid)
                {
                    FetchQuests();
                }

                if (!isFetchingDailyStats && !App.Stats.IsDailyStatsValid)
                {
                    FetchDailyStats();
                }

                if (!isUpdatingLifetimeStats && dailyStatsSyncStopwatch.ElapsedMilliseconds >= StatsSyncInterval)
                {
                    dailyStatsSyncStopwatch.Restart();
                    SyncStatsWithServer();
                }
            }
        }

        void SyncStatsWithServer()
        {
            isUpdatingLifetimeStats = true;

            App.Stats.UpdateLifetimeStats(success => isUpdatingLifetimeStats = false);
        }

        void FetchDailyStats()
        {
            isFetchingDailyStats = true;

            App.Stats.FetchStats(success =>
            {
                if (!App.Stats.IsDailyStatsValid)
                {
                    Invoke("FetchDailyStats", 1.0f);
                }

                isFetchingDailyStats = !App.Stats.IsDailyStatsValid;
            });
        }

        void FetchQuests()
        {
            isUpdatingQuests = true;

            App.Quests.FetchQuests(success =>
            {
                if (!App.Quests.IsDailyQuestsValid)
                {
                    Invoke("FetchQuests", 1.0f);
                }

                isUpdatingQuests = !App.Quests.IsDailyQuestsValid;
            });
        }
    }
}
