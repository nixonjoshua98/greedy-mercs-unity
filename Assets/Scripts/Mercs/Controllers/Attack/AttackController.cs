using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public abstract class AttackController : GM.Core.GMMonoBehaviour
    {
        [SerializeField] protected MercController Controller;
        [SerializeField] protected float CooldownTimer = 1.0f;

        protected UnitBase CurrentTarget;
        protected EnemyUnitCollection EnemyUnits => Controller.EnemyUnits;

        public bool IsAttacking { get; protected set; }
        public bool IsOnCooldown { get; protected set; }
        public bool HasControl { get; protected set; }

        public abstract bool IsWithinAttackDistance(GM.Units.UnitBase target);

        public virtual bool CanStartAttack(UnitBase unit)
        {
            return !IsOnCooldown && !IsAttacking && IsWithinAttackDistance(unit);
        }

        public virtual void StartAttack(GM.Units.UnitBase target)
        {
            IsAttacking = true;
            CurrentTarget = target;
        }

        protected void StartCooldown()
        {
            StartCoroutine(CooldownTask(CooldownTimer));
        }

        private IEnumerator CooldownTask(float seconds)
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(seconds);
            IsOnCooldown = false;
        }
    }
}
