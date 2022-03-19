using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    [System.Serializable]
    class ClaimButtonColours
    {
        public Color InProgress = GM.Common.Constants.Colors.Grey;
        public Color ReadyToComplete = GM.Common.Constants.Colors.Green;
    }

    public abstract class AbstractQuestSlot : MonoBehaviour
    {
        public TMP_Text Title;
        public Slider ProgressSlider;

        [Space]
        [SerializeField] ClaimButtonColours ButtonColours;
        [Space]
        public Button CompleteButton;
        public Image CompleteButtonImage;
        public GameObject CompletedOverlay;

        protected QuestsPopup Popup;
        [HideInInspector] public IAggregatedQuest Quest;

        public void Init(QuestsPopup popup, IAggregatedQuest quest)
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

        public abstract void ClaimButton_OnClick();
    }
}
