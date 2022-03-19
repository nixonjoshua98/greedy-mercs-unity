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

        bool isUpdatingDailyQuests;

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

                if (!isUpdatingDailyQuests && !App.Quests.IsDailyQuestsValid)
                {
                    FetchDailyQuests();
                }
            }
        }

        void FetchDailyQuests()
        {
            isUpdatingDailyQuests = true;

            App.Quests.FetchQuests(success =>
            {
                if (!success)
                {
                    Invoke("UpdateDailyQuests", 1.0f);
                }

                isUpdatingDailyQuests = !success;
            });
        }
    }
}
