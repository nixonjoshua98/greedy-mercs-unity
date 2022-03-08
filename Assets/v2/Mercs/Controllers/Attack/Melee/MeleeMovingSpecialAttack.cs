using GM.Controllers;
using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface ISpecialAttackController
    {
        bool HasControl { get; }
        bool WantsControl();
        void GiveControl();
    }

    public class MeleeMovingSpecialAttack : AbstractSpecialAttackController, ISpecialAttackController
    {
        [Header("Properties")]
        [SerializeField] float MoveSpeed = 10.0f;
        [SerializeField] float AttackRange = 0.5f;

        [Header("Components")]
        [SerializeField] UnitAvatar Avatar;
        [SerializeField] MercController Controller;
        [SerializeField] MovementController Movement;

        bool HasPerformedAttack;

        public override bool WantsControl()
        {
            UnitBaseClass current = null;

            if (HasPerformedAttack || !Controller.TryGetValidTarget(ref current))
            {
                return false;
            }

            HealthController health = current.GetCachedComponent<HealthController>();

            return health.Percent >= 1.0 && CanDefeatTargetOneHit(current);
        }

        public override void GiveControl()
        {
            HasControl = true;
            HasPerformedAttack = true;

            StartCoroutine(MovingAttack());
        }

        public override void RemoveControl()
        {
            HasControl = false;
        }

        IEnumerator MovingAttack()
        {
            UnitBaseClass target = null;
            Vector3 moveDir = Vector3.zero;

            Avatar.PlayAnimation(Avatar.Animations.MoveAttack);

            while (HasControl)
            {
                yield return new WaitForFixedUpdate();

                if (Controller.TryGetValidTarget(ref target))
                {
                    // This attck only works when we can defeat the enemy instantly
                    if (!CanDefeatTargetOneHit(target))
                    {
                        RemoveControl();
                        yield break;
                    }

                    moveDir = GetMoveDirection(target);

                    Movement.MoveDirection(moveDir, MoveSpeed, playAnimation: false);

                    if (Avatar.DistanceBetweenAvatar(target.Avatar) <= AttackRange)
                    {
                        Controller.PerformAttack(target);
                    }
                }
                else
                {
                    Movement.MoveDirection(moveDir, MoveSpeed, playAnimation: false);
                }
            }
        }

        bool CanDefeatTargetOneHit(UnitBaseClass unit)
        {
            HealthController health = unit.GetCachedComponent<HealthController>();

            return health.MaxHealth < Controller.MercDataValues.DamagePerAttack;
        }

        Vector3 GetMoveDirection(UnitBaseClass unit)
        {
            return (unit.transform.position - transform.position).normalized;
        }
    }
}