using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] Slider energySlider;

        [SerializeField] Text energyText;

        void FixedUpdate()
        {
            GameState.Player.currentEnergy = Math.Min(StatsCache.PlayerMaxEnergy(), GameState.Player.currentEnergy + ((StatsCache.PlayerEnergyPerMinute() / 60) * Time.fixedDeltaTime));

            UpdateUI();
        }

        void UpdateUI()
        {
            double maxEnergy = StatsCache.PlayerMaxEnergy();

            energyText.text = string.Format("Energy {0}/{1} ({2}/60s)", GameState.Player.currentEnergy.ToString("0.0"), maxEnergy, StatsCache.PlayerEnergyPerMinute().ToString("0.0"));

            energySlider.maxValue = (float)maxEnergy;

            energySlider.value = (float)GameState.Player.currentEnergy;
        }
    }
}
