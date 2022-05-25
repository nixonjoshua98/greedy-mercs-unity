using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MeleeAttackController : AttackController
    {
        public UnitAvatar Avatar;

        [Header("Prefabs")]
        public GameObject AttackImpactObject;

        [Header("Components (MeleeAttackController)")]
        [SerializeField] protected MovementController MoveController;

        [Header("Properties")]
        [SerializeField] private float AttackRange = 1.0f;

        private void Awake()
        {
            SubscribeToEvents();
            GetRequiredComponents();
        }

        protected virtual void SubscribeToEvents()
        {
            Avatar.E_Anim_MeleeAttackImpact.AddListener(Animation_AttackImpact);
            Avatar.E_Anim_MeleeAttackFinished.AddListener(Animation_AttackFinished);
        }

        public override void StartAttack(UnitBase target)
        {
            base.StartAttack(target);
            Avatar.PlayAnimation(Avatar.Animations.Attack);
        }

        public override bool IsWithinAttackDistance(GM.Units.UnitBase unit)
        {
            float dist = Avatar.DistanceXBetweenAvatar(unit.Avatar);

            return dist <= AttackRange;
        }

        private void InstantiateAttackImpactObject()
        {
            Instantiate(AttackImpactObject, CurrentTarget.Avatar.Bounds.RandomCenterPosition(), Quaternion.identity);
        }

        public override bool WantsControl()
        {
            if (!EnemyUnits.TryGetUnit(ref CurrentTarget))
                return false;

            return IsWithinAttackDistance(CurrentTarget);
        }

        public override void GiveControl()
        {
            HasControl = true;
            IsAttacking = false;

            StartCoroutine(UpdateLoop());
        }

        public override void RemoveControl()
        {
            if (HasControl)
            {
                HasControl = false;
                IsAttacking = false;
                CurrentTarget = null;
            }
        }

        private IEnumerator UpdateLoop()
        {
            while (HasControl && EnemyUnits.TryGetUnit(ref CurrentTarget))
            {
                if (!IsAttacking && !IsWithinAttackDistance(CurrentTarget))
                {
                    RemoveControl();
                    break;
                }

                // Start an attack (assuming we can)
                else if (CanStartAttack(CurrentTarget))
                {
                    StartAttack(CurrentTarget);
                }

                else if (IsOnCooldown)
                {
                    Avatar.PlayAnimation(Avatar.Animations.Idle);
                }

                yield return new WaitForEndOfFrame();
            }
        }


        // == Callbacks == //
        public void Animation_AttackImpact()
        {
            InstantiateAttackImpactObject();
            Controller.DealDamageToTarget(CurrentTarget);
        }

        public void Animation_AttackFinished()
        {
            StartCooldown();
            RemoveControl();
        }
    }
}
