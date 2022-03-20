using System;
using TMPro;
using UnityEngine;

namespace GM.Quests.UI
{
    public class DailyQuestsTab : GM.Core.GMMonoBehaviour
    {
        [Space]
        [SerializeField] GameObject QuestSlotObject;

        [Header("References")]
        [SerializeField] GameObject LoadingOverlay;
        [Space]
        [SerializeField] TMP_Text InfoText;
        [SerializeField] Transform QuestParent;

        void Awake()
        {
            InstantiateQuests();
        }


        void FixedUpdate()
        {
            bool isShowingQuests = !LoadingOverlay.activeInHierarchy && App.Quests.IsDailyQuestsValid && App.Stats.IsDailyStatsValid;

            LoadingOverlay.SetActive(!isShowingQuests);

            if (isShowingQuests)
            {
                var ts = App.Quests.NextDailyRefresh - DateTime.UtcNow;

                InfoText.text = ts.TotalSeconds <= 0.0f ? "Daily quests are refreshing" : $"Quests refresh in <color=orange>{ts.Format(TimeSpanFormat.Largest)}</color>";
            }
        }


        void InstantiateQuests()
        {
            foreach (var quest in App.Quests.DailyQuests)
            {
                var slot = Instantiate<AbstractQuestSlot<AggregatedDailyQuest>>(QuestSlotObject, QuestParent);

                slot.Init(ClaimDailyQuest, quest);
            }
        }


        public void ClaimDailyQuest(AbstractQuestSlot<AggregatedDailyQuest> slot)
        {
            App.Quests.SendCompleteDailyQuest(slot.Quest, success =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
