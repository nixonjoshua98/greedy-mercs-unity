using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    protected double maxHealth;
    protected double currentHealth;

    public bool IsDead { get { return currentHealth <= 0.0f; } }

    void Awake()
    {
        maxHealth = currentHealth = GetIntialHealth();
    }

    public abstract double GetIntialHealth();

    public virtual void TakeDamage(double amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;
        }
    }
}
