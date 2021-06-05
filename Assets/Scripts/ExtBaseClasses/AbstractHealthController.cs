using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using GM.Events;

namespace GM
{
    public abstract class AbstractHealthController : MonoBehaviour
    {
        BigDouble maxHealth;
        BigDouble currentHealth;

        // Events
        [HideInInspector] public GameObjectEvent E_OnDeath;
        [HideInInspector] public UnityEvent E_OnDamageTaken;

        void Awake()
        {
            E_OnDeath = new GameObjectEvent();

            maxHealth = currentHealth = GetIntialHealth();
        }

        public abstract BigDouble GetIntialHealth();

        public virtual void TakeDamage(BigDouble amount)
        {
            if (currentHealth > 0.0f)
            {
                currentHealth -= amount;

                if (currentHealth <= 0.0f)
                    E_OnDeath.Invoke(gameObject);

                else
                    E_OnDamageTaken.Invoke();
            }

            currentHealth = BigDouble.Max(0, currentHealth);
        }

        public float Percent()
        {
            return (float)(currentHealth / maxHealth).ToDouble();
        }
    }
}