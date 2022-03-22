using System.Collections;
using UnityEngine;

namespace GM
{
    public class ServerSyncManager : GM.Core.GMMonoBehaviour
    {
        bool isUpdatingQuests;
        bool isFetchingDailyStats;

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
            }
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
