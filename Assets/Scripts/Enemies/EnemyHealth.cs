using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    float maxHealth;
    float currentHealth;

    Slider healthbar;

    public bool IsDead {  get { return currentHealth <= 0.0f; } }

    void Awake()
    {
        SetHealth();

        GetHealthbar();

        healthbar.value = currentHealth / maxHealth;
    }

    public void SetHealth()
    {
        maxHealth = currentHealth = 5.0f;
    }

    private void GetHealthbar()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        healthbar.value = currentHealth / maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthbar.value = currentHealth / maxHealth;
    }
}
