using System.Collections.Generic;
using UnityEngine;

namespace GM.UI.HUD.StageDisplay
{
    public class StageDisplay : GM.Core.GMMonoBehaviour
    {
        [SerializeField] GameManager Manager;

        [SerializeField] List<StageDisplayIcon> DisplayIcons = new();

        void Start()
        {
            UpdateDisplayIcons();

            Manager.E_OnBossDefeated.AddListener(OnStageCompleted);
        }

        void UpdateDisplayIcons()
        {
            int initialStage = App.GameState.Stage - (DisplayIcons.Count / 2);

            for (int i = 0; i < DisplayIcons.Count; i++)
            {
                int stage = initialStage + i;

                DisplayIcons[i].UpdateIcon(stage <= 0 ? "" : stage.ToString());
            }
        }

        /* Callbacks */

        void OnStageCompleted()
        {
            UpdateDisplayIcons();
        }
    }
}
