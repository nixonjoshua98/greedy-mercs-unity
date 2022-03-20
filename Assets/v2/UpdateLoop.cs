using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace GM
{
    public class UpdateLoop : GM.Core.GMMonoBehaviour
    {
        static UpdateLoop Instance = null;

        bool isUpdatingQuests;
        bool isFetchingDailyStats;

        void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            StartCoroutine(_UpdateLoop());
        }

        IEnumerator _UpdateLoop()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (App is null)
                    continue;

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

            App.Stats.FetchDailyStats(success =>
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
