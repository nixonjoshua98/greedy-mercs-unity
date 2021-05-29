using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class PrestigeIcon : PanelIcon
    {
        [SerializeField] Animator anim;

        void Awake()
        {
            InvokeRepeating("RegularUpdate", 0.0f, 0.5f);
        }

        void RegularUpdate()
        {
            if (GameState.Stage.stage >= StageState.MIN_PRESTIGE_STAGE)
            {
                CancelInvoke("RegularUpdate");
            }
        }


        public override void OnClick()
        {
            anim.Play("Idle");

            if (GameState.Stage.stage >= StageState.MIN_PRESTIGE_STAGE)
                base.OnClick();

            else
            {
                Utils.UI.ShowMessage("Cashing Out", string.Format("Unlocks at stage {0}", StageState.MIN_PRESTIGE_STAGE));
            }
        }
    }
}