using UnityEngine;
using System.Collections.Generic;

namespace GM.Quests.UI
{
    public class QuestsPopup : GM.UI.PopupPanelBase
    {
        [Space]
        [SerializeField] GameObject MercQuestSlotObject;
        [SerializeField] GameObject DailyQuestSlotObject;

        [Header("References")]
        [SerializeField] Transform MercQuestsParent;
        [SerializeField] Transform DailyQuestsParent;

        void Awake()
        {
            InstantiateQuests(App.Quests.MercQuests, MercQuestSlotObject, MercQuestsParent);
            InstantiateQuests(App.Quests.DailyQuests, DailyQuestSlotObject, DailyQuestsParent);

            ShowInnerPanel();
        }

        void InstantiateQuests<T>(List<T> quests, GameObject slotObject, Transform parent) where T: IAggregatedQuest
        {
            for (int i = 0; i < quests.Count; i++)
            {
                var quest = quests[i];

                AbstractQuestSlot slot = Instantiate<AbstractQuestSlot>(slotObject, parent);

                slot.Init(this, quest);
            }
        }

        // = Callbacks = //

        public void ClaimMercQuest(MercQuestSlot slot)
        {
            App.Quests.SendCompleteMercQuest(slot.Quest, (success) =>
            {
                slot.OnClaimResponse(success);
            });
        }

        public void ClaimDailyQuest(DailyQuestSlot slot)
        {
            slot.OnClaimResponse(true);
        }
    }
}
