using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace GM
{
    public class HUD : Core.GMMonoBehaviour
    {
        [SerializeField]
        TMP_Text CurrentStageText;

        void FixedUpdate()
        {
            CurrentStageText.text = $"Stage {App.GMData.GameState.Stage}\n{App.GMData.GameState.EnemiesDefeated}/{App.GMData.GameState.EnemiesPerStage}";
        }
    }
}