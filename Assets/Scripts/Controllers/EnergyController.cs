
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class EnergyController : Core.GMMonoBehaviour
    {
        [SerializeField] Slider energySlider;

        [SerializeField] Text energyText;

        void FixedUpdate()
        {
            float energySinceLastUpdate = (StatsCache.EnergyPerMinute() / 60 * Time.fixedUnscaledDeltaTime);

            App.Data.Inv.Energy = Mathf.Min(StatsCache.MaxEnergyCapacity(), App.Data.Inv.Energy + energySinceLastUpdate);

            UpdateUI();
        }

        void UpdateUI()
        {
            float maxEnergy = StatsCache.MaxEnergyCapacity();

            energyText.text = $"Energy {App.Data.Inv.Energy:0.0}/{maxEnergy} ({StatsCache.EnergyPerMinute():0.0}/60s)";

            energySlider.maxValue = maxEnergy;

            energySlider.value = App.Data.Inv.Energy;
        }
    }
}
