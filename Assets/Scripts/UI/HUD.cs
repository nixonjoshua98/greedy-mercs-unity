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
            CurrentStageText.text = $"Stage {App.Data.GameState.Stage}\n{App.Data.GameState.EnemiesDefeated}/{App.Data.GameState.EnemiesPerStage}";
        }
    }
}