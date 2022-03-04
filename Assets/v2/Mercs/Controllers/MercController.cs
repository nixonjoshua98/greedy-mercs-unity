using GM.Common.Enums;
using UnityEngine.Events;
using GM.Units;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MercController : UnitBaseClass
    {
        public UnitID Id;

        [Header("Components")]
        [SerializeField] MovementController Movement;
        AttackController AttackController;

        // = Controllers = //
        UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Managers
        IEnemyUnitFactory UnitManager;
        GameManager GameManager;
        ISquadController SquadController;

        // Energy
        bool IsEnergyDepleted;
        int EnergyRemaining;

        // ...
        GM.Mercs.Data.MercData MercDataValues => App.GMData.Mercs.GetMerc(Id);


        void Awake()
        {
            GetComponents();
            SubscribeToEvents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        void SubscribeToEvents()
        {
            AttackController.E_AttackFinished.AddListener(AttackController_OnAttackFinished);
        }

        protected void GetComponents()
        {
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
            GameManager = this.GetComponentInScene<GameManager>();
            SquadController = this.GetComponentInScene<ISquadController>();

            AttackController = GetComponent<AttackController>();
        }

        void FixedUpdate()
        {
            if (!IsEnergyDepleted)
            {
                UpdateMercWithEnergy();
            }
            else
            {

            }
        }

        void UpdateMercWithEnergy()
        {
            int idx = SquadController.GetQueuePosition(this);

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
                        StartAttack();
                    }
                }
            }

            else if (idx > 0)
            {
                UnitBaseClass unit = SquadController.GetUnitAtQueuePosition(idx - 1);

                Vector3 targetPosition = new Vector3(unit.Avatar.Bounds.min.x - unit.Avatar.Bounds.size.x, transform.position.y);

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

        protected void StartAttack()
        {
            AttackController.StartAttack(CurrentTarget, OnAttackImpact);
        }

        void DealDamageToTarget()
        {
            BigDouble dmg = MercDataValues.DamagePerAttack;

            if (GameManager.DealDamageToTarget(dmg))
            {
                OnDamageDealt.Invoke(dmg);
            }
        }

        // = Callbacks = //

        protected void OnAttackImpact()
        {
            DealDamageToTarget();
        }

        void AttackController_OnAttackFinished()
        {
            EnergyRemaining -= MercDataValues.EnergyConsumedPerAttack;

            if (EnergyRemaining <= 0)
            {
                IsEnergyDepleted = true;

                SquadController.RemoveMercFromQueue(this);

                Destroy(gameObject);
            }
        }
    }
}
