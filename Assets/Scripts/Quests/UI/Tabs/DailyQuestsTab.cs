using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SRC.Quests.UI
{
    public class DailyQuestsTab : SRC.Core.GMMonoBehaviour
    {
        [Space]
        [SerializeField] private GameObject QuestSlotObject;

        [Header("References")]
        [SerializeField] private GameObject LoadingOverlay;
        [Space]
        [SerializeField] private TMP_Text InfoText;
        [SerializeField] private Transform QuestParent;
        private readonly List<DailyQuestSlot> Slots = new();

        private void Awake()
        {
            InstantiateQuests();
        }

        private void FixedUpdate()
        {
            bool isShowingQuests = !LoadingOverlay.activeInHierarchy && App.Quests.IsDailyQuestsValid;

            LoadingOverlay.SetActive(!isShowingQuests);

            if (isShowingQuests)
            {
                var ts = App.DailyRefresh.TimeUntilNext;

                InfoText.text = ts.TotalSeconds <= 1.0f ? "Daily quests are refreshing" : $"Quests refresh in <color=orange>{ts.ToString(TimeSpanFormat.Largest)}</color>";

                Slots.ForEach(s => s.UpdateUI());
            }
        }

        private void InstantiateQuests()
        {
            foreach (var quest in App.Quests.DailyQuests)
            {
                var slot = this.Instantiate<DailyQuestSlot>(QuestSlotObject, QuestParent);

                slot.Init(ClaimDailyQuest, quest);

                Slots.Add(slot);
            }
        }

        public void ClaimDailyQuest(DailyQuestSlot slot)
        {
            App.Quests.CompleteQuest(slot.Quest, success =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
