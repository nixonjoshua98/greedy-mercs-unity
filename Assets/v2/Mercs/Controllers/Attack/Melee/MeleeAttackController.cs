using GM.Units;
using System;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AttackController
    {
        public UnitAvatar Avatar;

        [Header("Prefabs")]
        public GameObject AttackImpactObject;

        [Header("Properties")]
        [SerializeField] float AttackRange = 0.5f;

        // = Controllers = //
        IMovementController MoveController;

        void Awake()
        {
            SetupEvents();
            GetComponents();
        }

        void SetupEvents()
        {
            var events = GetComponentInChildren<UnitAvatarAnimationEvents>();

            events.Attack.AddListener(OnMeleeAttackImpact);
        }

        void GetComponents()
        {
            MoveController = GetComponent<IMovementController>();
        }

        public override void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback)
        {
            base.StartAttack(target, callback);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
        }

        public override bool IsWithinAttackDistance(GM.Units.UnitBaseClass unit)
        {
            Vector3 position = GetTargetPositionFromTarget(unit);

            return Mathf.Abs(Avatar.Bounds.center.x - position.x) <= AttackRange;
        }

        public override void MoveTowardsAttackPosition(GM.Units.UnitBaseClass unit)
        {
            MoveController.MoveTowards(GetTargetPositionFromTarget(unit));
        }

        /// <summary>
        /// Fetch the target position from the unit provided. We use the Avatar to determine which side (Left or Right) to move towards
        /// </summary>
        Vector3 GetTargetPositionFromTarget(GM.Units.UnitBaseClass unit)
        {
            // Target is LEFT
            if (Avatar.Bounds.min.x > unit.Avatar.Bounds.max.x)
            {
                return new Vector3(unit.Avatar.Bounds.max.x + AttackRange, transform.position.y);
            }
            // Target is RIGHT
            else
            {
                return new Vector3(unit.Avatar.Bounds.min.x - AttackRange, transform.position.y);
            }
        }

        public void OnMeleeAttackImpact()
        {
            Cooldown();
            DealDamageToTarget();
            InstantiateAttackImpactObject();
        }

        void InstantiateAttackImpactObject()
        {
            Instantiate(AttackImpactObject, CurrentTarget.Avatar.Bounds.RandomCenterPosition());
        }
    }
}
