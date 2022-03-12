using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Quests.UI
{
    public class QuestsPopup : GM.UI.PopupPanelBase
    {
        [Space]
        [SerializeField] GameObject MercQuestSlotObject;

        [Header("References")]
        [SerializeField] Transform MercQuestsParent;

        void Awake()
        {
            InstantiateMercQuests();

            ShowInnerPanel();
        }

        void InstantiateMercQuests()
        {
            var quests = App.DataContainers.Quests.MercQuests;

            for (int i = 0; i < quests.Count; i++)
            {
                var quest = quests[i];

                MercQuestSlot slot = Instantiate<MercQuestSlot>(MercQuestSlotObject, MercQuestsParent);

                slot.Init(this, quest);
            }
        }

        // = Callbacks = //

        public void ClaimMercQuest(MercQuestSlot slot)
        {
            App.DataContainers.Quests.SendCompleteMercQuest(slot.Quest, (success) =>
            {
                slot.OnClaimResponse(success);
            });
        }
    }
}
