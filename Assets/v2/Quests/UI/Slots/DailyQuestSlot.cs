using System;
using TMPro;
using UnityEngine;

namespace GM.Quests.UI
{
    public class DailyQuestSlot : AbstractQuestSlot
    {
        public TMP_Text ClaimRewardText;

        [HideInInspector] public AggregatedDailyQuest Quest;

        Action<DailyQuestSlot> ClaimQuestFunction;

        public void Init(Action<DailyQuestSlot> claimQuestFunc, AggregatedDailyQuest quest)
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

        void SetStaticUI()
        {
            ClaimRewardText.text = Quest.DiamondsRewarded.ToString();

            Title.text = Quest.Title;
        }

        public void UpdateUI()
        {
            if (!Quest.IsCompleted)
            {
                ProgressSlider.value = Quest.CurrentProgress;
                CompleteButton.interactable = Quest.CurrentProgress >= 1.0f && !Quest.IsCompleted;
                CompleteButtonImage.color = Quest.CurrentProgress >= 1.0f ? ButtonColours.ReadyToComplete : ButtonColours.InProgress;
            }
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
