
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public interface IAttackController
    {
        public bool IsTargetValid(GM.Units.UnitBaseClass obj);
        public bool IsAvailable { get; }
        public bool IsAttacking { get; }

        void StartAttack(GM.Units.UnitBaseClass target, Action callback);
        bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);
        void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);
    }


    public abstract class AttackController : GM.Core.GMMonoBehaviour, IAttackController
    {
        [SerializeField]
        float CooldownTimer = 1.0f;

        protected GM.Units.UnitBaseClass CurrentTarget;
        Action DealDamageToTargetAction;

        // State
        protected bool _IsAttacking;
        public bool IsAttacking { get => _IsAttacking; }

        protected bool IsOnCooldown;
        public bool IsAvailable => !IsOnCooldown && !_IsAttacking;

        // Events
        [HideInInspector] public UnityEvent E_AttackFinished = new UnityEvent();

        public abstract bool IsWithinAttackDistance(GM.Units.UnitBaseClass target);
        public abstract void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target);

        public bool IsTargetValid(GM.Units.UnitBaseClass obj)
        {
            if (obj == null)
                return false;

            GM.Controllers.HealthController health = obj.GetComponent<GM.Controllers.HealthController>();

            return !health.IsDead;
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
            yield return new WaitForSeconds(CooldownTimer);
            IsOnCooldown = false;
        }
    }
}
