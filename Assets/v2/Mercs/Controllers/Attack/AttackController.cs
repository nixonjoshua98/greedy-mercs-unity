using GM.Targets;
using System;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsTargetValid(GameObject obj);
        public bool IsAvailable { get; }

        void StartAttack(Target target, Action<Target> callback);
        bool InAttackPosition(Target target);
        void MoveTowardsAttackPosition(Target target);
    }

    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        public float CooldownTimer = 1.0f;

        protected Target CurrentTarget;
        Action<Target> DealDamageToTargetAction;

        // = State Variables = //
        protected bool isAttacking;
        protected bool isOnCooldown;

        // = Properties = //
        public bool IsTargetValid(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }

            GM.Controllers.HealthController health = obj.GetComponent<GM.Controllers.HealthController>();

            return !health.IsDead;
        }

        public bool IsTargetValid() => IsTargetValid(CurrentTarget.GameObject ?? null);

        public bool IsAvailable => !isOnCooldown && !isAttacking;

        public abstract bool InAttackPosition(Target target);
        public abstract void MoveTowardsAttackPosition(Target target);

        public virtual void StartAttack(Target target, Action<Target> callback)
        {
            isAttacking = true;
            CurrentTarget = target;
            DealDamageToTargetAction = callback;
        }

        protected void DealDamageToTarget()
        {
            if (CurrentTarget == null || CurrentTarget.GameObject == null)
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
