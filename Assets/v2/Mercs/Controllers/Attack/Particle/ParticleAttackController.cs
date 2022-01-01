using GM.Targets;
using GM.Units;
using System;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class ParticleAttackController : AttackController
    {
        public UnitAvatar Avatar;
        [Space]
        public GameObject ParticleSystemObject;

        [Header("Properties")]
        public float AttackRange = 2;

        // = Controllers = //
        IMovementController MoveController;

        public override bool InAttackPosition(Target target)
        {
            return Mathf.Abs(target.Position.x - transform.position.x) < AttackRange;
        }

        public override void MoveTowardsAttackPosition(Target target)
        {
            MoveController.MoveTowards(target.Position);
        }

        public override void StartAttack(Target target, Action<Target> callback)
        {
            base.StartAttack(target, callback);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
        }

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
            InstantiateParticles();

            Invoke("OnAttackImpact", 0.25f);
        }

        protected virtual void InstantiateParticles()
        {
            if (ParticleSystemObject != null && CurrentTarget != null && CurrentTarget.GameObject != null)
                Instantiate(ParticleSystemObject, CurrentTarget.Avatar.AvatarCenter);
        }

        void OnAttackImpact()
        {
            DealDamageToTarget();
            Cooldown();
        }
    }
}
