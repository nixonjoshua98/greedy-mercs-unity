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

        void OnEnable() => InvokeRepeating("UpdateUI", 0.0f, 0.5f);

        void OnDisable() => CancelInvoke("UpdateUI");

        void Awake()
        {
            if (!GameState.Quests.IsValid)
            {
                GameState.Quests.Reset();
            }
        }

        void UpdateUI()
        {
            if (GameState.Quests.IsValid)
                dailyResetText.text = string.Format("Time until reset: {0}", Utils.Format.FormatSeconds(GameState.TimeUntilNextReset));

            else
                dailyResetText.text = "Resetting...";
        }
    }
}