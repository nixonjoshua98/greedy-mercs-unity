using GM.Controllers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI.HUD
{
    public class HealthBarHUD : MonoBehaviour
    {
        [SerializeField] GameManager GameManager;

        [Header("Components")]
        [SerializeField] Slider LaggingSlider;
        [SerializeField] Slider HealthSlider;
        [Space]
        [SerializeField] TMP_Text HealthText;

        bool _isUpdatingLaggingSlider;

        void Awake()
        {
            ResetHealthBar();
        }

        void Start()
        {
            GameManager.E_OnEnemySpawn.AddListener(OnEnemySpawned);
            GameManager.E_OnBossReady.AddListener(payload => OnEnemySpawned(payload.GameObject));
            GameManager.E_OnPreEnemyReady.AddListener(OnPreEnemyReady);
        }

        void ResetHealthBar()
        {
            HealthText.text = string.Empty;

            HealthSlider.value = 0;
            LaggingSlider.value = 0;
        }

        void UpdateHealthBar(HealthController health)
        {
            HealthText.text = Format.Number(health.Current, 1);
            HealthSlider.value = (float)health.Percentage;

            if (!_isUpdatingLaggingSlider)
            {
                StartCoroutine(UpdateLaggingSlider());
            }
        }

        void OnPreEnemyReady(GameObject payload)
        {
            var health = payload.GetComponent<HealthController>();

            this.Lerp01(0.25f, progress => HealthSlider.value = progress);

            HealthText.text = Format.Number(health.Current, 1);
        }

        void OnEnemySpawned(GameObject payload)
        {
            LaggingSlider.value = 1.0f;

            var health = payload.GetComponent<HealthController>();

            health.E_OnDamageTaken.AddListener((damage) => OnEnemyDamageTaken(payload, damage));

            UpdateHealthBar(health);
        }

        void OnEnemyDamageTaken(GameObject enemy, BigDouble damage)
        {
            var health = enemy.GetComponent<HealthController>();

            UpdateHealthBar(health);
        }

        /* Routines */

        IEnumerator UpdateLaggingSlider()
        {
            _isUpdatingLaggingSlider = true;

            while (LaggingSlider.value > HealthSlider.value)
            {
                LaggingSlider.value = Mathf.Lerp(LaggingSlider.value, HealthSlider.value, 2 * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            _isUpdatingLaggingSlider = false;
        }
    }
}
