
using System;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsTargetValid(GM.Units.UnitBaseClass obj);
        public bool IsAvailable { get; }
        public bool IsAttacking { get; }

        void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback);
        bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);
        void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);
    }


    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        [SerializeField]
        float CooldownTimer = 1.0f;

        protected GM.Units.UnitBaseClass CurrentTarget;
        Action<GM.Units.UnitBaseClass> DealDamageToTargetAction;

        // State
        protected bool _IsAttacking;
        public bool IsAttacking { get => _IsAttacking; }

        protected bool IsOnCooldown;
        public bool IsAvailable => !IsOnCooldown && !_IsAttacking;
        // ...


        public abstract bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);
        public abstract void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);

        public bool IsTargetValid(GM.Units.UnitBaseClass obj)
        {
            if (obj == null)
                return false;

            GM.Controllers.HealthController health = obj.GetComponent<GM.Controllers.HealthController>();

            return !health.IsDead;
        }

        public virtual void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback)
        {
            _IsAttacking = true;
            CurrentTarget = target;
            DealDamageToTargetAction = callback;
        }

        protected void DealDamageToTarget()
        {
            if (CurrentTarget == null)
            {
                GMLogger.Editor("Attempted to deal damage to destroyed target");
            }
            else
            {
                DealDamageToTargetAction(CurrentTarget);
            }
        }

        protected void Cooldown()
        {
            _IsAttacking = false;
            StartCooldown();
        }

        protected void StartCooldown()
        {
            StartCoroutine(CooldownTask());
        }

        IEnumerator CooldownTask()
        {
            IsOnCooldown = true;
            yield return new WaitForSecondsRealtime(CooldownTimer);
            IsOnCooldown = false;
        }
    }
}
