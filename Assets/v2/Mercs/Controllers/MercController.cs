using GM.Common.Enums;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public class MercController : Units.UnitBaseClass
    {
        public UnitID Id;

        // = Controllers = //
        GM.Units.UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Interfaces
        IAttackController AttackController;

        // Managers
        IEnemyUnitFactory UnitManager;
        GameManager GameManager;

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

            AttackController = GetComponent<IAttackController>();
        }

        void FixedUpdate()
        {
            if (!AttackController.IsTargetValid(CurrentTarget))
            {
                UnitManager.TryGetEnemyUnit(out CurrentTarget);
            }
            else
            {
                if (!AttackController.IsAttacking)
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
