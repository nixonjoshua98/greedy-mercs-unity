using SRC.Controllers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.UI.HUD
{
    public class HealthBarHUD : MonoBehaviour
    {
        [SerializeField] private GameManager GameManager;

        [Header("Components")]
        [SerializeField] private Slider LaggingSlider;
        [SerializeField] private Slider HealthSlider;
        [Space]
        [SerializeField] private TMP_Text HealthText;
        private bool _isUpdatingLaggingSlider;

        private void Awake()
        {
            ResetHealthBar();
        }

        private void Start()
        {
            GameManager.E_OnEnemySpawn.AddListener(OnEnemySpawned);
            GameManager.E_OnBossReady.AddListener(payload => OnEnemySpawned(payload.GameObject));
            GameManager.E_OnPreEnemyReady.AddListener(OnPreEnemyReady);
        }

        private void ResetHealthBar()
        {
            HealthText.text = string.Empty;

            HealthSlider.value = 0;
            LaggingSlider.value = 0;
        }

        private void UpdateHealthBar(HealthController health)
        {
            HealthText.text = Format.Number(health.Current, 1);
            HealthSlider.value = (float)health.Percentage;

            if (!_isUpdatingLaggingSlider)
            {
                StartCoroutine(UpdateLaggingSlider());
            }
        }

        private void OnPreEnemyReady(GameObject payload)
        {
            var health = payload.GetComponent<HealthController>();

            this.Lerp01(0.25f, progress => HealthSlider.value = progress);

            HealthText.text = Format.Number(health.Current, 1);
        }

        private void OnEnemySpawned(GameObject payload)
        {
            LaggingSlider.value = 1.0f;

            var health = payload.GetComponent<HealthController>();

            health.E_OnDamageTaken.AddListener((damage) => OnEnemyDamageTaken(payload, damage));

            UpdateHealthBar(health);
        }

        private void OnEnemyDamageTaken(GameObject enemy, BigDouble damage)
        {
            var health = enemy.GetComponent<HealthController>();

            UpdateHealthBar(health);
        }

        /* Routines */

        private IEnumerator UpdateLaggingSlider()
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
