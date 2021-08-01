using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class PrestigeIcon : PanelIcon
    {
        public override void OnClick()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (state.Stage >= StaticData.MIN_PRESTIGE_STAGE)
                base.OnClick();

            else
            {
                CanvasUtils.ShowInfo("Prestige", "You have not unlocked this yet");
            }
        }
    }
}