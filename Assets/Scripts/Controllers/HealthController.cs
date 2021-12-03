using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class HealthController : MonoBehaviour
    {
        public BigDouble MaxHealth { get; private set; }
        public BigDouble Current { get; private set; }

        public UnityEvent E_OnZeroHealth { get; set; } = new UnityEvent();
        public UnityEvent<BigDouble> E_OnDamageTaken { get; set; } = new UnityEvent<BigDouble>();

        public bool IsDead { get; private set; } = false;
        public float Percent => (float)(Current / MaxHealth).ToDouble();

        public void Setup(BigDouble val)
        {
            MaxHealth = Current = val;
        }

        public virtual void TakeDamage(BigDouble amount)
        {
            BigDouble damageTaken = amount > Current ? Current : amount;

            if (!IsDead)
            {
                Current -= amount;

                E_OnDamageTaken.Invoke(damageTaken);

                if (Current <= 0.0f)
                {
                    IsDead = true;

                    E_OnZeroHealth.Invoke();
                }
            }
        }
    }
}