using GM.Controllers;
using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AttackController
    {
        public UnitAvatar Avatar;

        [Header("Prefabs")]
        public GameObject AttackImpactObject;

        [Header("Components (MeleeAttackController)")]
        [SerializeField] protected MovementController MoveController;

        [Header("Properties")]
        [SerializeField] private float AttackRange = 1.0f;

        private void Awake()
        {
            SubscribeToEvents();
        }

        protected virtual void SubscribeToEvents()
        {
            Avatar.E_Anim_MeleeAttackImpact.AddListener(Animation_AttackImpact);
            Avatar.E_Anim_MeleeAttackFinished.AddListener(Animation_AttackFinished);
        }

        public override void StartAttack(UnitBase target)
        {
            base.StartAttack(target);

            if (target.TryGetComponent(out HealthController health))
            {
                health.E_OnZeroHealth.AddListener(() => CurrentTarget = null);
            }

            HasControl = true;

            Avatar.PlayAnimation(Avatar.Animations.Attack);

            StartCoroutine(_Update());
        }

        public override bool IsWithinAttackDistance(GM.Units.UnitBase unit)
        {
            return Avatar.DistanceXBetweenAvatar(unit.Avatar) <= AttackRange;
        }

        private void InstantiateAttackImpactObject()
        {
            Instantiate(AttackImpactObject, CurrentTarget.Avatar.Bounds.RandomCenterPosition(), Quaternion.identity);
        }

        public void Stop()
        {
            HasControl = false;
            IsAttacking = false;
            CurrentTarget = null;
        }

        IEnumerator _Update()
        {
            while (HasControl && IsTargetValid(CurrentTarget))
            {
                if (CanStartAttack(CurrentTarget))
                    StartAttack(CurrentTarget);

                else if (IsOnCooldown)
                    Avatar.PlayAnimation(Avatar.Animations.Idle);

                yield return new WaitForEndOfFrame();
            }

            Stop();
        }

        bool IsTargetValid(UnitBase unit)
        {
            return unit is not null && IsWithinAttackDistance(unit);
        }

        public void Animation_AttackImpact()
        {
            InstantiateAttackImpactObject();
            Controller.DealDamageToTarget();
        }

        public void Animation_AttackFinished()
        {
            IsAttacking = false;

            StartCooldown();
        }
    }
}
