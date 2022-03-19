namespace GM.Quests.UI
{
    public class MercQuestSlot : AbstractQuestSlot<AggregatedMercQuest>
    {     
        public override void ClaimButton_OnClick()
        {
            Popup.ClaimMercQuest(this);
        }
    }
}
