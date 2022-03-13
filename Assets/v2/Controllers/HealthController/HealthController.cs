using UnityEngine;
using UnityEngine.Events;

namespace GM.Controllers
{
    public abstract class AbstractHealthController : MonoBehaviour
    {
        // Events
        public UnityEvent E_OnZeroHealth { get; private set; } = new UnityEvent();
        public UnityEvent<BigDouble> E_OnDamageTaken { get; private set; } = new UnityEvent<BigDouble>();

        // Status
        public bool Invincible { get; set; } = false;
        public bool CanTakeDamage => !IsDead && !Invincible;
        public bool IsDead { get; private set; } = false;

        // Health
        public BigDouble MaxHealth { get; protected set; }
        public BigDouble CurrentHealth { get; protected set; }

        // ...
        public float Percent => (float)(CurrentHealth / MaxHealth).ToDouble();

        public virtual void TakeDamage(BigDouble value)
        {
            if (CanTakeDamage)
            {
                BigDouble dmgDealt = BigDouble.Min(value, CurrentHealth);

                CurrentHealth -= value;

                E_OnDamageTaken.Invoke(dmgDealt);

                if (CurrentHealth <= 0.0f)
                {
                    IsDead = true;

                    E_OnZeroHealth.Invoke();
                }
            }
        }
    }


    public class HealthController : AbstractHealthController
    {
        public void Init(BigDouble val)
        {
            MaxHealth = CurrentHealth = val;
        }
    }
}