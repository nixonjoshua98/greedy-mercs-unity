
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class HUD : Core.GMMonoBehaviour
    {
        [SerializeField] Text stageText;
        [SerializeField] Text waveText;

        void FixedUpdate()
        {
            stageText.text = $"Stage {App.Data.GameState.Stage}";
            waveText.text = $"{App.Data.GameState.Wave} / {App.Data.GameState.WavesPerStage}";
        }
    }
}