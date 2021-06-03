using System;
using System.Collections;
using System.Collections.Generic;

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
            GameState.Player.currentEnergy = Math.Min(StatsCache.MaxEnergyCapacity(), GameState.Player.currentEnergy + ((StatsCache.EnergyPerMinute() / 60) * Time.fixedDeltaTime));

            UpdateUI();
        }

        void UpdateUI()
        {
            double maxEnergy = StatsCache.MaxEnergyCapacity();

            energyText.text = string.Format("Energy {0}/{1} ({2}/60s)", GameState.Player.currentEnergy.ToString("0.0"), maxEnergy, StatsCache.EnergyPerMinute().ToString("0.0"));

            energySlider.maxValue = (float)maxEnergy;

            energySlider.value = (float)GameState.Player.currentEnergy;
        }
    }
}
