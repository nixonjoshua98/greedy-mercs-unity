using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] Slider healthbar;
    [SerializeField] Text healthText;

    void Awake()
    {
        EventManager.OnEnemyHurt.AddListener(OnEnemyHurt);

        EventManager.OnEnemySpawned.AddListener(OnEnemySpawned);

        EventManager.OnBossSpawned.AddListener(OnBossSpawned);
    }

    void OnEnemySpawned() { healthbar.value = 1.0f; }

    void OnBossSpawned(GameObject _) { healthbar.value = 1.0f; }

    void OnEnemyHurt(Health health)
    {
        healthbar.value = (float)(health.CurrentHealth / health.MaxHealth);

        healthText.text = health.CurrentHealth > 0 ? Utils.Format.FormatNumber(health.CurrentHealth) : "";
    }
}
