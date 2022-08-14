using UnityEngine;
using UnityEngine.Events;

namespace GM.Controllers
{
    public class HealthController : MonoBehaviour
    {
        // Events
        public UnityEvent E_OnZeroHealth { get; private set; } = new UnityEvent();
        public UnityEvent<BigDouble> E_OnDamageTaken { get; private set; } = new UnityEvent<BigDouble>();

        // Status
        public bool IsDead { get; private set; } = false;

        // Health
        public BigDouble MaxHealth { get; protected set; }
        public BigDouble Current { get; protected set; }

        // ...
        public float Percentage => (float)(Current / MaxHealth).ToDouble();

        public void Init(BigDouble val)
        {
            MaxHealth = Current = val;
        }

        public virtual void TakeDamage(BigDouble value)
        {
            if (!IsDead)
            {
                BigDouble dmgDealt = BigDouble.Min(value, Current);

                Current -= dmgDealt;

                E_OnDamageTaken.Invoke(dmgDealt);

                if (Current <= 0.0f)
                {
                    IsDead = true;

                    E_OnZeroHealth.Invoke();
                }
            }
        }
    }
}