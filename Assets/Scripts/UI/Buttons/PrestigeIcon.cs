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

            if (state.currentStage >= StaticData.MIN_PRESTIGE_STAGE)
                base.OnClick();

            else
            {
                Utils.UI.ShowMessage("Cashing Out", string.Format("Unlocks at stage {0}", StaticData.MIN_PRESTIGE_STAGE));
            }
        }
    }
}