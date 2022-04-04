using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public abstract class AttackController : AbstractUnitActionController, IUnitActionController
    {
        [Header("Components (AttackController)")]
        [SerializeField] protected MercController Controller;

        /* Scene Components */
        protected IEnemyUnitQueue EnemyUnits;

        [SerializeField] protected float CooldownTimer = 1.0f;

        protected UnitBaseClass CurrentTarget;

        public bool IsAttacking { get; protected set; }
        public bool IsOnCooldown { get; protected set; }

        public abstract bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);

        protected virtual void GetRequiredComponents()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitQueue>();
        }

        public virtual bool CanStartAttack(UnitBaseClass unit)
        {
            return !IsOnCooldown && !IsAttacking && IsWithinAttackDistance(unit);
        }

        public virtual void StartAttack(GM.Units.UnitBaseClass target)
        {
            IsAttacking = true;
            CurrentTarget = target;
        }

        protected void StartCooldown()
        {
            StartCooldown(CooldownTimer);
        }


        protected void StartCooldown(float secs)
        {
            StartCoroutine(CooldownTask(secs));
        }

        private IEnumerator CooldownTask(float seconds)
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(seconds);
            IsOnCooldown = false;
        }
    }
}
