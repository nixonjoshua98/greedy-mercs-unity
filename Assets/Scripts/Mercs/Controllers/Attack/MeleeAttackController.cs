using GM.Controllers;
using GM.Units;
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

        public override void StartAttack(GameObject target)
        {
            base.StartAttack(target);

            if (target.TryGetComponent(out HealthController health))
            {
                health.E_OnZeroHealth.AddListener(() => CurrentTarget = null);
            }

            Avatar.PlayAnimation(Avatar.Animations.Attack);
        }

        private void InstantiateAttackImpactObject()
        {
            var avatar = CurrentTarget.GetComponentInChildren<UnitAvatar>();

            Instantiate(AttackImpactObject, avatar.Bounds.RandomPosition(), Quaternion.identity);
        }

        public void Animation_AttackImpact()
        {
            if (CurrentTarget is not null)
            {
                InstantiateAttackImpactObject();
                Controller.DealDamageToTarget();
            }
        }

        public void Animation_AttackFinished()
        {
            StartCooldown();

            IsAttacking = false;
        }
    }
}
