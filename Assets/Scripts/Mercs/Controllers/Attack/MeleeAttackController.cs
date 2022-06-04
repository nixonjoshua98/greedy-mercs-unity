using GM.Controllers;
using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AbstractAttackController
    {
        [Header("Prefabs")]
        public GameObject AttackImpactObject;

        [Header("Components")]
        [SerializeField] protected MovementController MoveController;

        protected override void Awake()
        {
            base.Awake();

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
            while (HasControl && CurrentTarget != null)
            {
                if (CanStartAttack(CurrentTarget))
                    StartAttack(CurrentTarget);

                else if (IsOnCooldown)
                    Avatar.PlayAnimation(Avatar.Animations.Idle);

                yield return new WaitForEndOfFrame();
            }

            Stop();
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
