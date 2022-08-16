using TMPro;
using UnityEngine;

namespace SRC.Quests.UI
{
    public class MercQuestsTab : SRC.Core.GMMonoBehaviour
    {
        [Space]
        [SerializeField] private GameObject QuestSlotObject;

        [Header("References")]
        [SerializeField] private TMP_Text InfoText;
        [SerializeField] private Transform QuestParent;

        private void Awake()
        {
            InstantiateQuests();
        }

        private void OnEnable()
        {
            InfoText.text = "Unlock new mercenaries";
        }

        private void InstantiateQuests()
        {
            foreach (var quest in App.Quests.MercQuests)
            {
                var slot = this.Instantiate<MercQuestSlot>(QuestSlotObject, QuestParent);

                slot.Init(ClaimDailyQuest, quest);
            }
        }

        public void ClaimDailyQuest(MercQuestSlot slot)
        {
            App.Quests.CompleteQuest(slot.Quest, success =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
