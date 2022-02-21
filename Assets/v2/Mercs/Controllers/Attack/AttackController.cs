
using System;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsTargetValid(GM.Units.UnitBaseClass obj);
        public bool IsAvailable { get; }

        void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback);
        bool InAttackPosition(GM.Units.UnitBaseClass target);
        void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);
    }

    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        public float CooldownTimer = 1.0f;

        protected GM.Units.UnitBaseClass CurrentTarget;
        Action<GM.Units.UnitBaseClass> DealDamageToTargetAction;

        // = State Variables = //
        protected bool isAttacking;
        protected bool isOnCooldown;

        // = Properties = //
        public bool IsTargetValid(GM.Units.UnitBaseClass obj)
        {
            if (obj == null)
            {
                return false;
            }

            GM.Controllers.HealthController health = obj.GetComponent<GM.Controllers.HealthController>();

            return !health.IsDead;
        }

        public bool IsTargetValid() => IsTargetValid(CurrentTarget);

        public bool IsAvailable => !isOnCooldown && !isAttacking;

        public abstract bool InAttackPosition(GM.Units.UnitBaseClass target);
        public abstract void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);

        public virtual void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback)
        {
            isAttacking = true;
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
            isAttacking = false;

            StartCoroutine(CooldownTask());
        }

        IEnumerator CooldownTask()
        {
            isOnCooldown = true;
            yield return new WaitForSecondsRealtime(CooldownTimer);
            isOnCooldown = false;
        }
    }
}
