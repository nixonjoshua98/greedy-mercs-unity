using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TMP_Text HealthValueText;
        [SerializeField] Slider Slider;

        GM.Controllers.HealthController Health;
        Transform FollowTransformTarget;

        public void AssignHealthController(GM.Controllers.HealthController health, Transform followTransform)
        {
            Health = health;
            FollowTransformTarget = followTransform;

            // Events
            Health.E_OnDamageTaken.AddListener(OnHealthChange);
            Health.E_OnZeroHealth.AddListener(OnHealthZero);

            // Initial update
            UpdateHealthUI();
            UpdatePosition();

            // Disabled by default to prevent it being displayed when not ready
            gameObject.SetActive(true);
        }

        void FixedUpdate()
        {
            UpdatePosition();
        }

        void UpdateHealthUI()
        {
            HealthValueText.text = Format.Number(Health.CurrentHealth);
            Slider.value = Health.Percent;
        }

        void UpdatePosition()
        {
            if (FollowTransformTarget != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(FollowTransformTarget.position);
            }
        }

        // = Event Callbacks = // 

        void OnHealthChange(BigDouble damage)
        {
            UpdateHealthUI();
        }

        void OnHealthZero()
        {
            Destroy(gameObject);
        }
    }
}
