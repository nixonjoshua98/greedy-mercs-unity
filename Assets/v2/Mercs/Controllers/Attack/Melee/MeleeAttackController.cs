using GM.Targets;
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

            GMLogger.WhenNull(MoveController, "IMovementController is Null");
        }

        public override void StartAttack(Target target, Action<Target> callback)
        {
            base.StartAttack(target, callback);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
        }

        public override bool InAttackPosition(Target target)
        {
            return Vector2.Distance(transform.position, target.Position) <= AttackRange;
        }

        public override void MoveTowardsAttackPosition(Target target)
        {
            MoveController.MoveTowards(target.Position);
        }

        public void OnMeleeAttackImpact()
        {
            DealDamageToTarget();

            if (IsCurrentTargetValid)
            {
                InstantiateAttackImpactObject();
            }

            Cooldown();
        }

        void InstantiateAttackImpactObject()
        {
            Instantiate(AttackImpactObject, CurrentTarget.Avatar.AvatarCenter);
        }
    }
}
