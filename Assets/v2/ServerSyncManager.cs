using System.Collections;
using UnityEngine;
using System.Diagnostics;

namespace GM
{
    public class ServerSyncManager : GM.Core.GMMonoBehaviour
    {
        const float StatsSyncInterval = 1_000 * 30; // 30 Seconds

        bool isUpdatingQuests;
        bool isUpdatingLifetimeStats;

        Stopwatch syncStatsWatch;

        void Awake()
        {
            syncStatsWatch = Stopwatch.StartNew();
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

                if (!isUpdatingLifetimeStats && syncStatsWatch.ElapsedMilliseconds >= StatsSyncInterval)
                {
                    syncStatsWatch.Restart();

                    SyncStatsWithServer();
                }
            }
        }

        void SyncStatsWithServer()
        {
            isUpdatingLifetimeStats = true;

            App.Stats.UpdateLifetimeStats(success => isUpdatingLifetimeStats = false);
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
