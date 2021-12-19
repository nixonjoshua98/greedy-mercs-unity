using GM.Targets;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class StageHealthBarController : MonoBehaviour
    {
        public Slider primarySlider;
        public Slider secondarySlider;

        public Text healthValue;

        float targetSliderValue = 1.0f;

        void Awake()
        {
            SetupWaveSpawnEvent();
            SetupBossEvents();
        }

        void FixedUpdate()
        {
            primarySlider.value     = MathUtils.MoveTo(primarySlider.value, targetSliderValue, 1.25f * Time.fixedUnscaledDeltaTime);
            secondarySlider.value   = Mathf.Max(primarySlider.value, MathUtils.MoveTo(secondarySlider.value, targetSliderValue, 0.5f * Time.fixedUnscaledDeltaTime));
        }


        void SetupBossEvents()
        {
            GameManager.Instance.E_BossSpawn.AddListener(boss =>
            {
                targetSliderValue = 1.0f; // Fill up the slider to 100%

                // Set the current health
                healthValue.text = Format.Number(boss.Health.Current);

                // Update the display upon the boss takign damage
                boss.Health.E_OnDamageTaken.AddListener(damageTaken =>
                {
                    healthValue.text = Format.Number(boss.Health.Current);
                    targetSliderValue = boss.Health.Percent;
                });
            });
        }


        void SetupWaveSpawnEvent()
        {
            GameManager.Instance.E_OnWaveSpawn.AddListener(waveEnemies =>
            {
                BigDouble maxHealth = waveEnemies.Select(w => w.Health.MaxHealth).Sum();

                BigDouble current = maxHealth;

                healthValue.text = Format.Number(maxHealth);

                targetSliderValue = 1.0f;

                foreach (Target trgt in waveEnemies)
                {
                    trgt.Health.E_OnDamageTaken.AddListener(damageTaken =>
                    {
                        current -= damageTaken;

                        healthValue.text = Format.Number(current);
                        targetSliderValue = (float)(current / maxHealth).ToDouble();
                    });
                }
            });
        }
    }
}
