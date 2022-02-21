using UnityEngine;
using UnityEngine.Events;

namespace GM.Controllers
{
    public class HealthController : MonoBehaviour
    {
        public BigDouble MaxHealth { get; private set; }
        public BigDouble Current { get; private set; }

        public UnityEvent OnZeroHealth { get; set; } = new UnityEvent();
        public UnityEvent<BigDouble> OnDamageTaken { get; set; } = new UnityEvent<BigDouble>();

        public bool IsDead { get; private set; } = false;
        public float Percent => (float)(Current / MaxHealth).ToDouble();

        public void Init(BigDouble val)
        {
            MaxHealth = Current = val;
        }

        public virtual void TakeDamage(BigDouble amount)
        {
            if (!IsDead)
            {
                BigDouble dmgDealt = BigDouble.Min(amount, Current);

                Current -= amount;

                OnDamageTaken.Invoke(dmgDealt);

                if (Current <= 0.0f)
                {
                    IsDead = true;

                    OnZeroHealth.Invoke();
                }
            }
        }
    }
}