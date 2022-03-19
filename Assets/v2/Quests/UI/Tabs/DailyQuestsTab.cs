using TMPro;
using UnityEngine;

namespace GM.Quests.UI
{
    public class DailyQuestsTab : GM.Core.GMMonoBehaviour
    {
        [Space]
        [SerializeField] GameObject QuestSlotObject;

        [Header("References")]
        [SerializeField] TMP_Text InfoText;
        [SerializeField] Transform QuestParent;
        [SerializeField] GameObject RefreshOverlay;

        void Awake()
        {
            InstantiateQuests();
        }

        void FixedUpdate()
        {
            var ts = App.Quests.TimeUntilQuestsShouldRefresh;

            InfoText.text = ts.TotalSeconds <= 0.0f ? "Daily quests are refreshing" : $"Quests refresh in <color=orange>{ts.Format(TimeSpanFormat.Largest)}</color>";

            if (ts.TotalSeconds <= 3.0f)
            {
                RefreshOverlay.SetActive(true);
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
