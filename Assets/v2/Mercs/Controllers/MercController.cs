using GM.Common.Enums;
using GM.Controllers;
using GM.Targets;
using UnityEngine.Events;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public interface IMercController
    {
        UnityEvent<BigDouble> OnDamageDealt { get; set; }
    }

    public class MercController : Units.UnitBaseClass, IMercController
    {
        public MercID Id;

        // = Controllers = //
        IAttackController AttackController;

        Target CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        ITargetManager TargetManager;

        void Awake()
        {
            GetComponents();
        }

        protected override void GetComponents()
        {
            base.GetComponents();

            AttackController = GetComponent<IAttackController>();
            TargetManager = this.GetComponentInScene<ITargetManager>();         

            GMLogger.WhenNull(TargetManager, "Fatal: TargetManager is Null");
            GMLogger.WhenNull(Movement, "IMovementController is Null");
            GMLogger.WhenNull(AttackController, "IAttackController is Null");
        }

        void FixedUpdate()
        {
            if (CurrentTarget == null || !AttackController.IsTargetValid(CurrentTarget.GameObject))
            {
                TargetManager.TryGetMercTarget(ref CurrentTarget);
            }
            else
            {
                if (AttackController.IsAvailable)
                {
                    if (!AttackController.InAttackPosition(CurrentTarget))
                    {
                        AttackController.MoveTowardsAttackPosition(CurrentTarget);
                    }
                    else
                    {
                        StartAttack();
                    }
                }
            }
        }

        void StartAttack()
        {
            AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
        }

        protected void DealDamageToTarget(Target attackTarget)
        {
            if (attackTarget.GameObject.TryGetComponent(out HealthController health))
            {
                BigDouble dmg = App.Data.Mercs.GetMerc(Id).DamagePerAttack;

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);

                OnDamageDealt.Invoke(dmg);
            }
        }
    }
}
