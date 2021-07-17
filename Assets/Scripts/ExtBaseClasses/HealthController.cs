using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using GM.Events;

namespace GM
{
    public interface IHealthController
    {
        public void TakeDamage(BigDouble amount);
    }


    public abstract class HealthController : MonoBehaviour, IHealthController
    {
        public BigDouble MaxHealth { get; private set; }

        BigDouble currentHealth;

        // Events
        [HideInInspector] public UnityEvent E_OnDeath;
        [HideInInspector] public BigDoubleEvent E_OnDamageTaken;

        void Awake()
        {
            E_OnDeath       = new UnityEvent();
            E_OnDamageTaken = new BigDoubleEvent();

            MaxHealth = currentHealth = GetIntialHealth();
        }

        public abstract BigDouble GetIntialHealth();

        public virtual void TakeDamage(BigDouble amount)
        {
            if (currentHealth > 0.0f)
            {
                currentHealth -= amount;

                E_OnDamageTaken.Invoke(amount);

                if (currentHealth <= 0.0f)
                    E_OnDeath.Invoke();
            }
        }

        public float Percent()
        {
            return (float)(currentHealth / MaxHealth).ToDouble();
        }
    }
}