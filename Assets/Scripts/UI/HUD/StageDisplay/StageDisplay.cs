using System.Collections.Generic;
using UnityEngine;

namespace SRC.UI.HUD.StageDisplay
{
    public class StageDisplay : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GameManager Manager;

        [SerializeField] private List<StageDisplayIcon> DisplayIcons = new();

        private void Start()
        {
            UpdateDisplayIcons();

            Manager.E_OnBossDefeated.AddListener(OnStageCompleted);
        }

        private void UpdateDisplayIcons()
        {
            int initialStage = App.GameState.Stage - (DisplayIcons.Count / 2);

            for (int i = 0; i < DisplayIcons.Count; i++)
            {
                int stage = initialStage + i;

                DisplayIcons[i].UpdateIcon(stage <= 0 ? "" : stage.ToString());
            }
        }

        /* Callbacks */

        private void OnStageCompleted()
        {
            UpdateDisplayIcons();
        }
    }
}
