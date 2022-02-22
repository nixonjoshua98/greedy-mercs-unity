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

            Health.OnDamageTaken.AddListener(OnHealthChange);
            Health.OnZeroHealth.AddListener(OnHealthZero);

            UpdateHealthUI();
        }

        void FixedUpdate()
        {
            if (FollowTransformTarget != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(FollowTransformTarget.position);
            }
        }

        void UpdateHealthUI()
        {
            HealthValueText.text = Format.Number(Health.Current);
            Slider.value = Health.Percent;
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
