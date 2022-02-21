
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

        public override void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback)
        {
            base.StartAttack(target, callback);

            Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
        }

        public override bool InAttackPosition(GM.Units.UnitBaseClass target)
        {
            return Vector2.Distance(transform.position, target.transform.position) <= AttackRange;
        }

        public override void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target)
        {
            MoveController.MoveTowards(target.transform.position);
        }

        public void OnMeleeAttackImpact()
        {
            Cooldown();
            DealDamageToTarget();

            if (IsTargetValid())
            {
                InstantiateAttackImpactObject();
            }
        }

        void InstantiateAttackImpactObject()
        {
            Instantiate(AttackImpactObject, CurrentTarget.transform);
        }
    }
}
