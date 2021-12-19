using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Targets;
using GM.Units;
using System;

namespace GM.Mercs.Controllers
{
    public class ParticleAttackController : AttackController
    {
        public UnitAvatar Avatar;
        [Space]
        public GameObject SummonObject;

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

            events.Attack.AddListener(OnAttackAnimation);
        }

        void GetComponents()
        {
            MoveController = GetComponent<IMovementController>();

            GMLogger.WhenNull(MoveController, "IMovementController is Null");
        }

        void OnAttackAnimation()
        {
            InstantiateSummoningObject();
        }

        void InstantiateSummoningObject()
        {
            Avatar.Animator.speed = 0.0f;

            Instantiate(SummonObject, CurrentTarget.Avatar.AvatarCenter);

            Invoke("OnSummonedObject", 0.25f);
        }

        void OnSummonedObject()
        {
            Avatar.Animator.speed = 1.0f;

            DealDamageToTarget();
            Cooldown();
        }

        public override bool InAttackPosition(Target target)
        {
            return Mathf.Abs(target.Position.x - transform.position.x) < 5;
        }

        public override void MoveTowardsAttackPosition(Target target)
        {
            MoveController.MoveTowards(target.Position);
        }

        public override void StartAttack(Target target, Action callback)
        {
            base.StartAttack(target, callback);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
        }
    }
}
