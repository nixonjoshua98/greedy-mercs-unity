using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] Slider healthbar;

    void Awake()
    {
        EventManager.OnEnemyHurt.AddListener(OnEnemyHurt);

        EventManager.OnEnemySpawned.AddListener(OnEnemySpawned);
    }

    void OnEnemySpawned()
    {
        healthbar.value = 1.0f;
    }

    void OnEnemyHurt(Health health)
    {
        healthbar.value = (float)(health.CurrentHealth / health.MaxHealth);
    }
}
