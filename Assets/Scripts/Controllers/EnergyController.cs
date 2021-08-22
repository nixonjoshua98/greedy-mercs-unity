
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] Slider energySlider;

        [SerializeField] Text energyText;

        void FixedUpdate()
        {
            float energySinceLastUpdate = (StatsCache.EnergyPerMinute() / 60 * Time.fixedUnscaledDeltaTime);

            UserData.Get.Inventory.Energy = Mathf.Min(StatsCache.MaxEnergyCapacity(), UserData.Get.Inventory.Energy + energySinceLastUpdate);

            UpdateUI();
        }

        void UpdateUI()
        {
            float maxEnergy = StatsCache.MaxEnergyCapacity();

            energyText.text = $"Energy {UserData.Get.Inventory.Energy:0.0}/{maxEnergy} ({StatsCache.EnergyPerMinute():0.0}/60s)";

            energySlider.maxValue = maxEnergy;

            energySlider.value = UserData.Get.Inventory.Energy;
        }
    }
}
