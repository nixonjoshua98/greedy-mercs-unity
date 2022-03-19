using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GM.Quests.UI
{
    public class MercQuestsTab : GM.Core.GMMonoBehaviour
    {
        [Space]
        [SerializeField] GameObject QuestSlotObject;

        [Header("References")]
        [SerializeField] TMP_Text InfoText;
        [SerializeField] Transform QuestParent;

        void Awake()
        {
            InstantiateQuests();
        }

        void OnEnable()
        {
            InfoText.text = "Unlock new mercenaries";
        }

        void InstantiateQuests()
        {
            foreach (var quest in App.Quests.MercQuests)
            {
                var slot = Instantiate<AbstractQuestSlot<AggregatedMercQuest>>(QuestSlotObject, QuestParent);

                slot.Init(ClaimDailyQuest, quest);
            }
        }

        public void ClaimDailyQuest(AbstractQuestSlot<AggregatedMercQuest> slot)
        {
            App.Quests.SendCompleteMercQuest(slot.Quest, success =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
