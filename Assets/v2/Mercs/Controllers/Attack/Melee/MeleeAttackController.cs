using GM.Targets;
using GM.Units;
using UnityEngine;
using System;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AttackController
    {
        public UnitAvatar Avatar;

        [Header("Prefabs")]
        [Tooltip("Optional prefab to instantiate upon attack impact")]
        public GameObject AttackImpactObject;

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
            return target.Position == transform.position;
        }

        public override void MoveTowardsAttackPosition(Target target)
        {
            MoveController.MoveTowards(target.Position);
        }

        public void OnMeleeAttackImpact()
        {
            Cooldown();
            DealDamageToTarget();
            InstantiateAttackImpactObject();
        }

        void InstantiateAttackImpactObject()
        {
            if (AttackImpactObject != null)
            {
                Instantiate(AttackImpactObject, CurrentTarget.Avatar.AvatarCenter);
            }
        }
    }
}
