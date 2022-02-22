using GM.Common.Enums;
using GM.Controllers;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public class MercController : Units.UnitBaseClass
    {
        public MercID Id;

        // = Controllers = //
        GM.Units.UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Interfaces
        GM.Mercs.Controllers.IAttackController AttackController;

        // Scene Interfaces
        GM.Common.Interfaces.IUnitManager UnitManager;
        GM.UI.IDamageNumberManager DamageNumberManager;


        void Awake()
        {
            GetComponents();
        }

        protected void GetComponents()
        {
            DamageNumberManager = this.GetComponentInScene<GM.UI.IDamageNumberManager>();
            UnitManager = this.GetComponentInScene<GM.Common.Interfaces.IUnitManager>();

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
                if (AttackController.IsAvailable)
                {
                    if (!AttackController.IsWithinAttackDistance(CurrentTarget))
                    {
                        AttackController.MoveTowardsAttackPosition(CurrentTarget);
                    }
                    else
                    {
                        AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
                    }
                }
            }
        }

        protected void DealDamageToTarget(GM.Units.UnitBaseClass attackTarget)
        {
            if (attackTarget.TryGetComponent(out HealthController health))
            {
                BigDouble dmg = App.Data.Mercs.GetMerc(Id).DamagePerAttack;

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);

                OnDamageDealt.Invoke(dmg);

                DamageNumberManager.InstantiateNumber(attackTarget.Avatar, dmg);
            }
        }
    }
}
