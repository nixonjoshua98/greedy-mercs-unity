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

        void Start()
        {
            SetupWaveSpawnEvent();
            SetupBossEvents();
        }

        void Update()
        {
            primarySlider.value     = MathUtils.MoveTo(primarySlider.value, targetSliderValue, 1.25f * Time.unscaledDeltaTime);
            secondarySlider.value   = Mathf.Max(primarySlider.value, MathUtils.MoveTo(secondarySlider.value, targetSliderValue, 0.5f * Time.unscaledDeltaTime));
        }


        void SetupBossEvents()
        {
            GameManager.Instance.E_BossSpawn.AddListener(boss =>
            {
                targetSliderValue = 1.0f; // Fill up the slider to 100%

                // Set the current health
                healthValue.text = Format.Number(boss.Health.CurrentHealth);

                // Update the display upon the boss takign damage
                boss.Health.E_OnDamageTaken.AddListener(damageTaken =>
                {
                    healthValue.text = Format.Number(boss.Health.CurrentHealth);
                    targetSliderValue = boss.Health.Percent();
                });

                // Display the boss has been defeated
                boss.Health.E_OnZeroHealth.AddListener(() =>
                {
                    healthValue.text = "CLEAR";
                });
            });
        }


        void SetupWaveSpawnEvent()
        {
            GameManager.Instance.E_OnWaveSpawn.AddListener(payload =>
            {
                BigDouble current = payload.CombinedHealth;

                healthValue.text = Format.Number(payload.CombinedHealth);

                targetSliderValue = 1.0f;

                foreach (HealthController hp in payload.HealthControllers)
                {
                    // Update the slider/text upon the enemy taking damage
                    hp.E_OnDamageTaken.AddListener(damageTaken =>
                    {
                        current -= damageTaken;

                        healthValue.text = Format.Number(current);
                        targetSliderValue = (float)(current / payload.CombinedHealth).ToDouble();
                    });
                }
            });

            // Display a wave clear message
            GameManager.Instance.E_OnWaveCleared.AddListener(() =>
            {
                healthValue.text = "CLEAR";
            });
        }
    }
}
