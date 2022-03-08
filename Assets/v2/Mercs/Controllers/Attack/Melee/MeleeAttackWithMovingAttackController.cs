using GM.Units;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using GM.Controllers;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackWithMovingAttackController : MeleeAttackController
    {
        [Header("Running Attack Properties")]
        [SerializeField] float PostAttackCooldown = 1.0f;

        bool hasPerformedMovingAttack;

        // ...
        IEnemyUnitFactory UnitManager;

        Action<UnitBaseClass> DealDamageToTarget;

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Avatar.E_Anim_MeleeMoveAttackImpact.AddListener(Animation_MeleeMoveAttackImpact);
        }

        protected override void GetRequiredComponents()
        {
            base.GetRequiredComponents();
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
        }

        public override void TryGiveControl(int queuePosition, Action<UnitBaseClass> dealDamageAction)
        {
            if (!hasPerformedMovingAttack && !HasControl && queuePosition == 0)
            {
                HasControl = true;
                hasPerformedMovingAttack = true;
                DealDamageToTarget = dealDamageAction;

                StartCoroutine(MovingAttack());
            }
        }

        void GiveBackControl()
        {
            HasControl = false;
            _IsAttacking = false;
            StartCooldown(PostAttackCooldown);

            Avatar.PlayAnimation(Avatar.Animations.Idle);
        }

        IEnumerator MovingAttack()
        {
            Avatar.PlayAnimation(Avatar.Animations.MoveAttack);

            while (HasControl)
            {
                yield return new WaitForFixedUpdate();

                if (UnitManager.TryGetEnemyUnit(out CurrentTarget))
                {
                    MoveController.MoveTowards(GetTargetPositionFromTarget(CurrentTarget), playAnimation: false);
                }
                else
                {
                    MoveController.Continue();
                }
            }
        }

        // = Animation Callbacks = //

        void Animation_MeleeMoveAttackImpact()
        {
            if (!HasControl)
                return; // Exit early


            if (IsWithinAttackDistance(CurrentTarget))
            {
                DealDamageToTarget.Invoke(CurrentTarget);

                HealthController health = CurrentTarget.GetCachedComponent<HealthController>();

                if (!health.IsDead)
                {
                    GiveBackControl();
                }
            }
        }
    }
}
