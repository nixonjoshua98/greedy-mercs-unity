using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class PrestigeIcon : PanelIcon
    {
        public override void OnClick()
        {
            C_GameState state = GameManager.Instance.GetState();

            if (state.currentStage >= StaticData.MIN_PRESTIGE_STAGE)
                base.OnClick();

            else
            {
                Utils.UI.ShowMessage("Cashing Out", string.Format("Unlocks at stage {0}", StaticData.MIN_PRESTIGE_STAGE));
            }
        }
    }
}