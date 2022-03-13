using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    [System.Serializable]
    struct ClaimButtonColours
    {
        public Color InProgress;
        public Color ReadyToComplete;
    }

    public class MercQuestSlot : QuestSlot
    {
        [Space]
        [SerializeField] ClaimButtonColours ButtonColours;
        [Space]
        public Button CompleteButton;
        public Image CompleteButtonImage;
        public GameObject CompletedOverlay;

        QuestsPopup Popup;
        [HideInInspector] public AggregatedUserMercQuest Quest;

        public void Init(QuestsPopup popup, AggregatedUserMercQuest quest)
        {
            Quest = quest;
            Popup = popup;

            Title.text = quest.Title;

            if (Quest.IsCompleted)
            {
                UpdateWhenCompleted();
            }
        }


        void FixedUpdate()
        {
            if (!Quest.IsCompleted)
            {
                ProgressSlider.value = Quest.CurrentProgress;

                CompleteButton.interactable = Quest.CurrentProgress >= 1.0f && !Quest.IsCompleted;

                CompleteButtonImage.color = Quest.CurrentProgress >= 1.0f ? ButtonColours.ReadyToComplete : ButtonColours.InProgress;
            }
        }

        public void OnClaimResponse(bool success)
        {
            if (success)
            {
                UpdateWhenCompleted();
            }
        }

        void UpdateWhenCompleted()
        {
            ProgressSlider.value = 1.0f;
            Destroy(CompleteButton.gameObject);
            CompletedOverlay.SetActive(true);
        }

        // Callbacks //

        public void ClaimButton_OnClick()
        {
            Popup.ClaimMercQuest(this);
        }
    }
}
