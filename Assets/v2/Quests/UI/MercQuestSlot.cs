using UnityEngine;
using UnityEngine.UI;

namespace GM.Quests.UI
{
    public class MercQuestSlot : AbstractQuestSlot
    {     
        public override void ClaimButton_OnClick()
        {
            Popup.ClaimMercQuest(this);
        }
    }
}
