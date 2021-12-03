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
        public BigDouble Current { get; private set; }

        [HideInInspector] public UnityEvent E_OnZeroHealth { get; set; } = new UnityEvent();
        [HideInInspector] public UnityEvent<BigDouble> E_OnDamageTaken { get; set; } = new UnityEvent<BigDouble>();

        public bool IsDead = false;

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


        public float Percent()
        {
            return (float)(Current / MaxHealth).ToDouble();
        }
    }
}