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
        [SerializeField] float AttackRange = 1.0f;

        void Awake()
        {
            SubscribeToEvents();
            GetRequiredComponents();
        }

        protected virtual void SubscribeToEvents()
        {
            Avatar.E_Anim_MeleeAttackImpact.AddListener(Animation_AttackImpact);
            Avatar.E_Anim_MeleeAttackFinished.AddListener(Animation_AttackFinished);
        }

        public override void StartAttack(UnitBaseClass target)
        {
            base.StartAttack(target);
            Avatar.PlayAnimation(Avatar.Animations.Attack);
        }

        public override bool IsWithinAttackDistance(GM.Units.UnitBaseClass unit)
        {
            float dist = Avatar.DistanceBetweenAvatar(unit.Avatar);

            return dist <= AttackRange;
        }

        void InstantiateAttackImpactObject()
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
                CurrentTarget = null;
            }
        }

        IEnumerator UpdateLoop()
        {
            while (HasControl && EnemyUnits.TryGetUnit(ref CurrentTarget))
            {
                if (!IsAttacking && !IsWithinAttackDistance(CurrentTarget))
                    break;

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

            RemoveControl();
        }


        // == Callbacks == //
        public void Animation_AttackImpact()
        {
            Controller.DealDamageToTarget(CurrentTarget);
            InstantiateAttackImpactObject();
        }

        public void Animation_AttackFinished()
        {
            StartCooldown();
            RemoveControl();
        }
    }
}
