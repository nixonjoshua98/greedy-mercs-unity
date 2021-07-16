using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    [RequireComponent(typeof(HealthController))]
    public class HealthBarTarget : MonoBehaviour
    {
        [SerializeField] Slider HealthBarObject;

        [Header("References")]
        [SerializeField, Tooltip("Optional")] Transform _TargetTransform;

        Transform TargetTransform { get { return _TargetTransform ? _TargetTransform : transform; } }

        Slider slider;

        // - Components
        HealthController health;

        void Awake()
        {
            health = GetComponent<HealthController>();
        }

        void Start()
        {
            SubscribeToEvents();

            InstantiateText();
        }

        void OnDestroy()
        {
            if (slider)
                Destroy(slider.gameObject);
        }

        void SubscribeToEvents()
        {
            health.E_OnDamageTaken.AddListener(OnDamageTaken);
        }

        void InstantiateText()
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

            slider = Instantiate(HealthBarObject);

            slider.transform.SetParent(canvas.transform);

            UpdateText();
        }

        void LateUpdate()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(TargetTransform.position);
        }

        void UpdateText()
        {
            slider.value = health.Percent();
        }

        void OnDamageTaken()
        {
            UpdateText();
        }
    }

}