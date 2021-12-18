using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Targets;
using GM.Units;
using GM.Controllers;
using System;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AttackController
    {
        public UnitAvatar Avatar;

        // Controller Interfaces
        public IMovementController MoveController;


        bool isAttacking;
        Target CurrentTarget;

        Action DealDamageToTarget;

        public override bool IsAttacking() => isAttacking;

        void Awake()
        {
            SetupEvents();
            GetComponents();
        }

        void SetupEvents()
        {
            var events = GetComponentInChildren<MeleeUnitAnimationEvents>();

            events.AttackImpact.AddListener(OnMeleeAttackImpact);
        }

        void GetComponents()
        {
            MoveController = GetComponent<IMovementController>();

            GMLogger.WhenNull(MoveController, "IMovementController is Null");
        }

        public override void StartAttack(Target target, Action impactCallback)
        {
            isAttacking = true;
            CurrentTarget = target;
            DealDamageToTarget = impactCallback;

            Avatar.PlayerAnimation(Avatar.AnimationStrings.Attack);
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
            isAttacking = false;

            if (CurrentTarget.GameObject == null)
            {
                GMLogger.Editor("Attempted to deal damage to destroyed target");
            }
            else
            {
                DealDamageToTarget();
            }
        }
    }
}
