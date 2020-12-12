﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator anim;

    protected float maxHealth;
    protected float currentHealth;

    Slider healthbar;

    public bool IsDead {  get { return currentHealth <= 0.0f; } }

    void Awake()
    {
        SetHealth();

        GetHealthbar();

        healthbar.value = currentHealth / maxHealth;
    }

    public virtual void SetHealth()
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
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;

            healthbar.value = currentHealth / maxHealth;

            if (currentHealth <= 0.0f)
            {
                EnemyController controller = GetComponent<EnemyController>();

                controller.OnDeath();
            }
            else
            {
                anim.Play("Hurt");
            }
        }
    }
}
