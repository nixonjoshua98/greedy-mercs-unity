using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsAvailable { get; }

        public void StartAttack(Target target, Action callback);
        public bool InAttackPosition(Target target);
        public void MoveTowardsAttackPosition(Target target);
    }

    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        protected Target CurrentTarget;
        Action DealDamageToTargetAction;

        protected bool isAttacking;
        protected bool isOnCooldown;
        public bool IsAvailable => !isOnCooldown && !isAttacking;

        public abstract bool InAttackPosition(Target target);
        public abstract void MoveTowardsAttackPosition(Target target);

        public virtual void StartAttack(Target target, Action callback)
        {
            isAttacking = true;
            CurrentTarget = target;
            DealDamageToTargetAction = callback;
        }

        protected void DealDamageToTarget()
        {
            if (CurrentTarget.GameObject == null)
            {
                GMLogger.Editor("Attempted to deal damage to destroyed target");
            }
            else
            {
                DealDamageToTargetAction();
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
            yield return new WaitForSecondsRealtime(0.25f);
            isOnCooldown = false;
        }
    }
}
