
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] Text GoldText;

        [SerializeField] Text stageText;
        [SerializeField] Text waveText;

        void FixedUpdate()
        {
            CurrentStageState state = GameManager.Instance.State();

            GoldText.text = FormatString.Number(UserData.Get.Inventory.Gold);

            stageText.text = $"Stage {state.Stage}";
            waveText.text = $"{state.Wave} / {state.WavesPerStage}";
        }
    }
}