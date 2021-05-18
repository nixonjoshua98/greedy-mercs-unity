using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GM.Events;

namespace GreedyMercs
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] Animator anim;

        BigDouble maxHealth;
        BigDouble currentHealth;
        public BigDouble MaxHealth { get { return maxHealth; } }
        public BigDouble CurrentHealth { get { return currentHealth; } }

        public bool IsDead { get { return currentHealth <= 0.0f; } }

        public GameObjectEvent OnDeath;

        void Awake()
        {
            maxHealth = currentHealth = GetIntialHealth();
        }

        public abstract BigDouble GetIntialHealth();

        public virtual void TakeDamage(BigDouble amount)
        {
            if (currentHealth > 0.0f)
            {
                currentHealth -= amount;

                anim.Play("Hurt");

                if (currentHealth <= 0.0f)
                {
                    OnDeath.Invoke(gameObject);
                }
            }

            currentHealth = currentHealth < 0 ? 0 : currentHealth;

            anim.Play("Hurt");
        }
    }
}