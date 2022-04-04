using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace GM
{
    public class ServerSyncManager : GM.Core.GMMonoBehaviour
    {
        private const float StatsSyncInterval = 1_000 * 30; // 30 Seconds

        private bool isUpdatingQuests;
        private bool isUpdatingLifetimeStats;
        private Stopwatch syncStatsWatch;

        private void Awake()
        {
            syncStatsWatch = Stopwatch.StartNew();
        }

        private void Start()
        {
            StartCoroutine(_UpdateLoop());
        }

        private IEnumerator _UpdateLoop()
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

        private void SyncStatsWithServer()
        {
            isUpdatingLifetimeStats = true;

            App.Stats.UpdateLifetimeStats(success => isUpdatingLifetimeStats = false);
        }

        private void FetchQuests()
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
