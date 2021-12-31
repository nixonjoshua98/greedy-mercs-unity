using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsAvailable { get; }

        void Reset();

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
        protected bool IsCurrentTargetValid => !(CurrentTarget == null || CurrentTarget.GameObject == null || CurrentTarget.Health.IsDead);
        public bool IsAvailable => !isOnCooldown && !isAttacking;

        public abstract bool InAttackPosition(Target target);
        public abstract void MoveTowardsAttackPosition(Target target);

        public virtual void StartAttack(Target target, Action<Target> callback)
        {
            isAttacking = true;
            CurrentTarget = target;
            DealDamageToTargetAction = callback;
        }

        public void Reset()
        {
            isAttacking = false;
            CurrentTarget = null;
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
            isOnCooldown = true;
            StartCoroutine(CooldownTask());
        }

        IEnumerator CooldownTask()
        {
            yield return new WaitForSecondsRealtime(CooldownTimer);
            isOnCooldown = false;
        }
    }
}
