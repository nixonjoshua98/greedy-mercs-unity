using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    public class PrestigeIcon : PanelIcon
    {
        public override void OnClick()
        {
            if (GameState.Stage.stage >= StageState.MIN_PRESTIGE_STAGE)
                base.OnClick();

            else
            {
                Utils.UI.ShowMessage("Cashing Out", string.Format("Cashing out is unlocked at stage {0}", StageState.MIN_PRESTIGE_STAGE));
            }
        }
    }
}