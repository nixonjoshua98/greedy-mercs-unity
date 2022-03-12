using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    public class MercQuestSlot : QuestSlot
    {
        public Button CompleteButton;
        public TMP_Text ButtonText;

        QuestsPopup Popup;
        [HideInInspector] public AggregatedUserMercQuest Quest;

        public void Init(QuestsPopup popup, AggregatedUserMercQuest quest)
        {
            Quest = quest;
            Popup = popup;

            Title.text = quest.Title;
        }


        void FixedUpdate()
        {
            ProgressSlider.value = Quest.CurrentProgress;
            CompleteButton.interactable = Quest.CurrentProgress >= 1.0f && !Quest.IsCompleted;

            ButtonText.text = Quest.CurrentProgress >= 1.0f ? Quest.IsCompleted ? "" : "Claim" : "In\nProgress";
        }

        public void OnClaimResponse(bool success)
        {

        }

        // Callbacks //

        public void ClaimButton_OnClick()
        {
            Popup.ClaimMercQuest(this);
        }
    }
}
