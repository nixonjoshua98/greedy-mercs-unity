
using GM.Events;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public interface IHealthController
    {
        void Setup(BigDouble val);
        void TakeDamage(BigDouble amount);
    }


    public class HealthController : MonoBehaviour, IHealthController
    {
        public BigDouble MaxHealth { get; private set; }
        public BigDouble CurrentHealth { get; private set; }

        // Events
        [HideInInspector] public UnityEvent E_OnZeroHealth;
        [HideInInspector] public BigDoubleEvent E_OnDamageTaken;

        void Awake()
        {
            E_OnZeroHealth = new UnityEvent();
            E_OnDamageTaken = new BigDoubleEvent();
        }


        public void Setup(BigDouble val)
        {
            MaxHealth = CurrentHealth = val;
        }


        public virtual void TakeDamage(BigDouble amount)
        {
            BigDouble damageTaken = amount > CurrentHealth ? CurrentHealth : amount;

            if (CurrentHealth > 0.0f)
            {
                CurrentHealth = BigDouble.Max(0, CurrentHealth - amount);

                E_OnDamageTaken.Invoke(damageTaken);

                if (CurrentHealth <= 0.0f)
                    E_OnZeroHealth.Invoke();
            }
        }


        public float Percent()
        {
            return (float)(CurrentHealth / MaxHealth).ToDouble();
        }
    }
}