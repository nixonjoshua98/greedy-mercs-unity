using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GM.Quests.UI
{
    [System.Serializable]
    class ClaimButtonColours
    {
        public Color InProgress = GM.Common.Constants.Colors.Grey;
        public Color ReadyToComplete = GM.Common.Constants.Colors.Green;
    }

    public abstract class AbstractQuestSlot<T> : MonoBehaviour where T: IAggregatedQuest
    {
        public TMP_Text Title;
        public Slider ProgressSlider;

        [Space]
        [SerializeField] ClaimButtonColours ButtonColours;
        [Space]
        public Button CompleteButton;
        public Image CompleteButtonImage;
        public GameObject CompletedOverlay;

        [HideInInspector] public T Quest;

        Action<AbstractQuestSlot<T>> ClaimQuestFunction;

        public void Init(Action<AbstractQuestSlot<T>> claimQuestFunc, T quest)
        {
            Quest = quest;
            ClaimQuestFunction = claimQuestFunc;

            SetStaticUI();
            UpdateUI();

            if (Quest.IsCompleted)
            {
                UpdateWhenCompleted();
            }
        }


        void FixedUpdate()
        {
            if (!Quest.IsCompleted)
            {
                UpdateUI();
            }
        }

        protected virtual void SetStaticUI()
        {
            Title.text = Quest.Title;
        }

        protected virtual void UpdateUI()
        {
            ProgressSlider.value = Quest.CurrentProgress;

            CompleteButton.interactable = Quest.CurrentProgress >= 1.0f && !Quest.IsCompleted;

            CompleteButtonImage.color = Quest.CurrentProgress >= 1.0f ? ButtonColours.ReadyToComplete : ButtonColours.InProgress;
        }

        void UpdateWhenCompleted()
        {
            ProgressSlider.value = 1.0f;
            Destroy(CompleteButton.gameObject);
            CompletedOverlay.SetActive(true);
        }

        // Callbacks //

        public void OnClaimResponse(bool success)
        {
            if (success)
            {
                UpdateWhenCompleted();
            }
        }

        public void ClaimButton_OnClick()
        {
            ClaimQuestFunction.Invoke(this);
        }
    }
}
