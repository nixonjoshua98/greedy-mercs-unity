using GM.Common.Enums;
using UnityEngine.Events;
using GM.Units;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MercController : Units.UnitBaseClass
    {
        public UnitID Id;

        [Header("Components")]
        [SerializeField] MovementController Movement;

        // = Controllers = //
        GM.Units.UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Interfaces
        IAttackController AttackController;

        // Managers
        IEnemyUnitFactory UnitManager;
        GameManager GameManager;
        ISquadController SquadController;

        // ...
        GM.Mercs.Data.MercData MercDataValues => App.GMData.Mercs.GetMerc(Id);


        void Awake()
        {
            GetComponents();
        }

        protected void GetComponents()
        {
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
            GameManager = this.GetComponentInScene<GameManager>();
            SquadController = this.GetComponentInScene<ISquadController>();

            AttackController = GetComponent<IAttackController>();
        }

        void FixedUpdate()
        {
            int idx = SquadController.GetQueuePosition(Id);

            if (idx == 0)
            {
                if (!AttackController.IsTargetValid(CurrentTarget))
                {
                    UnitManager.TryGetEnemyUnit(out CurrentTarget);
                }
                else if (!AttackController.IsAttacking)
                {
                    if (!AttackController.IsWithinAttackDistance(CurrentTarget))
                    {
                        AttackController.MoveTowardsAttackPosition(CurrentTarget);
                    }

                    else if (AttackController.IsAvailable)
                    {
                        AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
                    }
                }
            }

            else if (idx > 0)
            {
                UnitBaseClass unit = SquadController.GetUnitAtQueuePosition(idx - 1);

                Vector3 targetPosition = unit.Avatar.Bounds.min - new Vector3(3, 0);

                if (transform.position != targetPosition)
                {
                    Movement.MoveTowards(targetPosition);
                }
                else
                {
                    Avatar.PlayAnimation(Avatar.Animations.Idle);
                }
            }
        }

        protected void DealDamageToTarget(GM.Units.UnitBaseClass attackTarget)
        {
            BigDouble dmg = MercDataValues.DamagePerAttack;

            if (GameManager.DealDamageToTarget(dmg))
            {
                OnDamageDealt.Invoke(dmg);
            }
        }
    }
}
