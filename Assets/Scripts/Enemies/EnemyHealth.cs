using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyHealth : Health
{
    [SerializeField] Animator anim;

    Slider healthbar;

    void Start()
    {
        GetHealthbar();

        healthbar.value = (float)(currentHealth / maxHealth);
    }

    public virtual void OnDeath()
    {
        if (TryGetComponent(out EnemyLoot loot))
        {
            loot.Process();
        }

        Destroy(gameObject);
    }

    private void GetHealthbar()
    {
        healthbar = GameObject.FindGameObjectWithTag("EnemyHealthbar").GetComponent<Slider>();

        healthbar.value = (float)(currentHealth / maxHealth);
    }

    public override void TakeDamage(double amount)
    {
        base.TakeDamage(amount);

        healthbar.value = (float)(currentHealth / maxHealth);

        if (IsDead)
            OnDeath();

        else
            anim.Play("Hurt");
    }
}
