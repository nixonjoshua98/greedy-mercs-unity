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
        protected IEnemyUnitCollection EnemyUnits;

        [SerializeField] protected float CooldownTimer = 1.0f;

        protected UnitBase CurrentTarget;

        public bool IsAttacking { get; protected set; }
        public bool IsOnCooldown { get; protected set; }

        public abstract bool IsWithinAttackDistance(GM.Units.UnitBase target);

        protected virtual void GetRequiredComponents()
        {
            EnemyUnits = this.GetComponentInScene<IEnemyUnitCollection>();
        }

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
