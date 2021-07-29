using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    using TMPro;

    public class HUD : MonoBehaviour
    {
        [SerializeField] Text GoldText;

        [SerializeField] TMP_Text stageWaveText;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.State();

            GoldText.text = FormatString.Number(GameState.Player.gold);

            stageWaveText.SetText($"Stage {state.Stage} | Wave {state.Wave} / {state.WavesPerStage}");
        }
    }
}