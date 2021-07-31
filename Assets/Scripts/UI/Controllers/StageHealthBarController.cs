using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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
            secondarySlider.value   = Mathf.Max(primarySlider.value, MathUtils.MoveTo(secondarySlider.value, targetSliderValue, 0.25f * Time.unscaledDeltaTime));
        }


        void SetupBossEvents()
        {
            GameManager.Get.E_OnBossSpawn.AddListener(boss =>
            {
                targetSliderValue = 1.0f;

                HealthController hp = boss.GetComponent<HealthController>();

                healthValue.text = FormatString.Number(hp.CurrentHealth);

                hp.E_OnDamageTaken.AddListener(damageTaken =>
                {
                    healthValue.text = FormatString.Number(hp.CurrentHealth);
                    targetSliderValue = hp.Percent();
                });

                hp.E_OnZeroHealth.AddListener(() =>
                {
                    healthValue.text = "CLEAR";
                });
            });
        }


        void SetupWaveSpawnEvent()
        {
            GameManager.Get.E_OnWaveSpawn.AddListener(payload =>
            {
                BigDouble current = payload.CombinedHealth;

                healthValue.text = FormatString.Number(payload.CombinedHealth);

                targetSliderValue = 1.0f;

                foreach (HealthController hp in payload.HealthControllers)
                {
                    hp.E_OnDamageTaken.AddListener(damageTaken =>
                    {
                        current -= damageTaken;

                        healthValue.text = FormatString.Number(current);
                        targetSliderValue = (float)(current / payload.CombinedHealth).ToDouble();
                    });
                }
            });


            GameManager.Get.E_OnWaveCleared.AddListener(() =>
            {
                healthValue.text = "CLEAR";
            });
        }
    }
}
