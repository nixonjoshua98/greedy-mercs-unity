
using GM.Controllers;
using GM.Units;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public abstract class AttackController : GM.Core.GMMonoBehaviour
    {
        [SerializeField]
        float CooldownTimer = 1.0f;

        protected GM.Units.UnitBaseClass CurrentTarget;
        Action DealDamageToTargetAction;

        // State
        protected bool _IsAttacking;
        public bool HasControl { get; protected set; }
        public bool IsAttacking { get => _IsAttacking; }

        protected bool IsOnCooldown;

        // Events
        [HideInInspector] public UnityEvent E_AttackFinished = new UnityEvent();

        public virtual void TryGiveControl(int queuePosition, Action<UnitBaseClass> damageImpact) { }
        public abstract bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);
        public abstract void MoveTowardsTarget(GM.Units.UnitBaseClass target);

        public virtual bool CanStartAttack(UnitBaseClass unit)
        {
            return !IsOnCooldown && !IsAttacking && IsWithinAttackDistance(unit);
        }

        public virtual void StartAttack(GM.Units.UnitBaseClass target, Action callback)
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
                DealDamageToTargetAction.Invoke();
            }
        }

        protected void StartCooldown()
        {
            StartCooldown(CooldownTimer);
        }

        protected void StartCooldown(float secs)
        {
            StartCoroutine(CooldownTask(secs));
        }


        IEnumerator CooldownTask(float seconds)
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(seconds);
            IsOnCooldown = false;
        }
    }
}
