using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Quests.UI
{
    public class QuestPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text dailyResetText;

        void Awake()
        {
            if (!GameState.Quests.IsValid)
            {
                GameState.Quests.Reset();
            }
        }

        void FixedUpdate()
        {
            if (GameState.Quests.IsValid)
                dailyResetText.text = string.Format("Time until reset: {0}", Funcs.Format.Seconds(Funcs.TimeUntil(StaticData.NextDailyReset).TotalSeconds));

            else
                dailyResetText.text = "Resetting...";
        }
    }
}