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
    public double MaxHealth { get { return maxHealth; } }
    public double CurrentHealth {  get { return currentHealth; } }

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

            anim.Play("Hurt");
        }

        currentHealth = currentHealth < 0 ? 0 : currentHealth;

        anim.Play("Hurt");
    }
}
