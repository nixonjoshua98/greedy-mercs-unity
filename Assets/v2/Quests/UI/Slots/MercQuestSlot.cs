using System;
using System.Collections;
using UnityEngine;

namespace GM.Quests.UI
{
    public class MercQuestSlot : AbstractQuestSlot
    {
        [HideInInspector] public AggregatedMercQuest Quest;

        Action<MercQuestSlot> ClaimQuestFunction;

        public void Init(Action<MercQuestSlot> claimQuestFunc, AggregatedMercQuest quest)
        {
            Quest = quest;
            ClaimQuestFunction = claimQuestFunc;

            SetStaticUI();
            UpdateUI();

            if (Quest.IsCompleted)
            {
                UpdateWhenCompleted();
            }
            else
            {
                StartCoroutine(_Update());
            }
        }

        IEnumerator _Update()
        {
            while (!Quest.IsCompleted && Quest.CurrentProgress < 1.0f)
            {
                UpdateUI();

                yield return new WaitForFixedUpdate();
            }
        }

        void SetStaticUI()
        {
            Title.text = Quest.Title;
        }

        void UpdateUI()
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
