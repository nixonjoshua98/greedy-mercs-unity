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
    }

    void Start()
    {
        GetHealthbar();

        healthbar.value = (float)(currentHealth / maxHealth);
    }

    public abstract double GetIntialHealth();

    public virtual void TakeDamage(double amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;

            anim.Play("Hurt");
        }

        healthbar.value = (float)(currentHealth / maxHealth);

        anim.Play("Hurt");
    }

    private void GetHealthbar()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        healthbar.value = (float)(currentHealth / maxHealth);
    }
}
