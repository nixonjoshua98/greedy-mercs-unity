using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    public class DailyQuestSlot : AbstractQuestSlot
    {       
        public override void ClaimButton_OnClick()
        {
            Popup.ClaimDailyQuest(this);
        }
    }
}
