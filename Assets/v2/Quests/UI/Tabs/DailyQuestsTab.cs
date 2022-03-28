using System.Collections.Generic;
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

        List<DailyQuestSlot> Slots = new();

        void Awake()
        {
            InstantiateQuests();
        }

        void FixedUpdate()
        {
            bool isShowingQuests = !LoadingOverlay.activeInHierarchy && App.Quests.IsDailyQuestsValid;

            LoadingOverlay.SetActive(!isShowingQuests);

            if (isShowingQuests)
            {
                var ts = App.DailyRefresh.TimeUntilNext;

                InfoText.text = ts.TotalSeconds <= 1.0f ? "Daily quests are refreshing" : $"Quests refresh in <color=orange>{ts.Format(TimeSpanFormat.Largest)}</color>";

                Slots.ForEach(s => s.UpdateUI());
            }
        }

        void InstantiateQuests()
        {
            foreach (var quest in App.Quests.DailyQuests)
            {
                var slot = Instantiate<DailyQuestSlot>(QuestSlotObject, QuestParent);

                slot.Init(ClaimDailyQuest, quest);

                Slots.Add(slot);
            }
        }

        public void ClaimDailyQuest(DailyQuestSlot slot)
        {
            App.Quests.SendCompleteDailyQuest(slot.Quest, success =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
