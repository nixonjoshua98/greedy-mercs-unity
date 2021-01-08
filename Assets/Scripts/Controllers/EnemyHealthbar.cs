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
        Events.OnEnemyHurt.AddListener(OnEnemyHurt);

        Events.OnEnemySpawned.AddListener(OnEnemySpawned);

        Events.OnBossSpawned.AddListener(OnBossSpawned);
    }

    void OnEnemySpawned() { healthbar.value = 1.0f; }

    void OnBossSpawned(GameObject _) { healthbar.value = 1.0f; }

    void OnEnemyHurt(Health health)
    {
        healthbar.value = float.Parse((health.CurrentHealth / health.MaxHealth).ToString());

        healthText.text = health.CurrentHealth > 0 ? Utils.Format.FormatNumber(health.CurrentHealth) : "";
    }
}
