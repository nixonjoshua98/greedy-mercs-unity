using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text HealthValueText;
        [SerializeField] private Slider Slider;

        protected GM.Controllers.HealthController Health;
        protected Transform FollowTransformTarget;

        public void AssignHealthController(GM.Controllers.HealthController health, Transform followTransform)
        {
            FollowTransformTarget = followTransform;

            Setup(health);
        }

        protected void Setup(GM.Controllers.HealthController health)
        {
            Health = health;

            // Events
            Health.E_OnDamageTaken.AddListener(OnHealthChange);
            Health.E_OnZeroHealth.AddListener(OnHealthZero);

            // Initial update
            UpdateHealthUI();
            UpdatePosition();

            // Disabled by default to prevent it being displayed when not ready
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            UpdatePosition();
        }

        private void UpdateHealthUI()
        {
            HealthValueText.text = Format.Number(Health.CurrentHealth);
            Slider.value = Health.Percent;
        }

        private void UpdatePosition()
        {
            if (FollowTransformTarget != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(FollowTransformTarget.position);
            }
        }

        // = Event Callbacks = // 

        private void OnHealthChange(BigDouble damage)
        {
            UpdateHealthUI();
        }

        protected virtual void OnHealthZero()
        {
            Destroy(gameObject);
        }
    }
}
