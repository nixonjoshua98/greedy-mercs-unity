using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth
{
    float maxHealth;
    float currentHealth;

    Slider healthbar;

    public bool IsDead {  get { return currentHealth <= 0.0f; } }

    public EnemyHealth()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        Reset();
    }

    public void Reset()
    {
        maxHealth = currentHealth = 5.0f;

        healthbar.value = 1.0f;
    }

    public bool TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthbar.value = currentHealth / maxHealth;

        return currentHealth <= 0.0f;
    }
}
