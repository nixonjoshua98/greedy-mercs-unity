using TMPro;

namespace GM.Quests.UI
{
    public class DailyQuestSlot : AbstractQuestSlot<AggregatedDailyQuest>
    {
        public TMP_Text ClaimRewardText;

        protected override void SetStaticUI()
        {
            base.SetStaticUI();
            ClaimRewardText.text = Quest.DiamondsRewarded.ToString();
        }
    }
}
