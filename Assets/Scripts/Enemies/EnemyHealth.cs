using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator anim;

    protected double maxHealth;
    protected double currentHealth;

    Slider healthbar;

    public bool IsDead {  get { return currentHealth <= 0.0f; } }

    void Awake()
    {
        SetHealth();

        GetHealthbar();

        healthbar.value = (float)(currentHealth / maxHealth);
    }

    public virtual double GetIntialHealth()
    {
        return GameManager.CurrentStage;
    }

    void SetHealth()
    {
        maxHealth = currentHealth = GetIntialHealth();
    }

    private void GetHealthbar()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        healthbar.value = (float)(currentHealth / maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;

            healthbar.value = (float)(currentHealth / maxHealth);

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
