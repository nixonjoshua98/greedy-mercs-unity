using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    [SerializeField] Animator anim;

    Slider healthbar;

    double maxHealth;
    double currentHealth;

    public bool IsDead { get { return currentHealth <= 0.0f; } }

    void Awake()
    {
        maxHealth = currentHealth = GetIntialHealth();

        GetHealthbar();
    }

    void Start()
    {
        UpdateHealthbar();
    }

    public abstract double GetIntialHealth();

    public virtual void TakeDamage(double amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;

            anim.Play("Hurt");
        }

        UpdateHealthbar();

        anim.Play("Hurt");
    }

    void UpdateHealthbar()
    {
        if (healthbar != null)
        {
            healthbar.value = (float)(currentHealth / maxHealth);
        }
    }

    void GetHealthbar()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        UpdateHealthbar();
    }
}
