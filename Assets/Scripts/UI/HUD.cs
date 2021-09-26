
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class HUD : Core.GMMonoBehaviour
    {
        [SerializeField] Text GoldText;

        [SerializeField] Text stageText;
        [SerializeField] Text waveText;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.State();

            GoldText.text = FormatString.Number(App.Data.Inv.Gold);

            stageText.text = $"Stage {state.Stage}";
            waveText.text = $"{state.Wave} / {state.WavesPerStage}";
        }
    }
}