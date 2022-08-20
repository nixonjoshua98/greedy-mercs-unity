using SRC.Common;
using SRC.Common.Enums;
using SRC.Controllers;
using SRC.Mercs.Data;
using SRC.Units;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SRC
{
    internal record MercAttackValues
    {
        public DamageType Type;
        public BigDouble Value;
    }
}


namespace SRC.Mercs.Controllers
{
    public abstract class AbstractMercController : SRC.Core.GMMonoBehaviour
    {
        public MercID ID = MercID.UNKNOWN;
        public SRC.Units.UnitAvatar Avatar;
        [Header("Components")]
        [SerializeField] public MovementController Movement;
        [SerializeField] private AbstractAttackController Attack;

        [Header("Events")]
        [HideInInspector] public UnityEvent<GameObject, BigDouble> E_OnDamageDealt = new();
        [HideInInspector] public UnityEvent E_OnEnemyDefeated = new();

        // Scene instances
        private SRC.DamageTextPool.DamageTextPool DamageNumbers;

        /* Init Values */
        protected MercSquadController SquadController;

        public bool InControl = true;

        /* Game Values */
        public SRC.Mercs.Data.AggregatedMercData DataValues => App.Mercs.GetMerc(ID);

        protected GameObject CurrentTarget;
        private Func<GameObject> TryGetTarget;

        public void Init(MercSquadController squad, Func<GameObject> getTargetFunc)
        {
            TryGetTarget = getTargetFunc;
            SquadController = squad;
        }

        private void Awake()
        {
            GetRequiredComponents();
        }

        protected void GetRequiredComponents()
        {
            DamageNumbers = this.GetComponentInScene<SRC.DamageTextPool.DamageTextPool>();
        }

        private void FixedUpdate()
        {
            if (!InControl)
            {
                return;
            }

            if (FetchTarget() && Attack.CanStartAttack)
            {
                Attack.StartAttack(CurrentTarget);
            }
        }

        protected virtual bool FetchTarget()
        {
            CurrentTarget = TryGetTarget();

            return CurrentTarget != null;
        }

        public void DealDamageToTarget()
        {
            var attackValues = CalculateAttackValue();

            HealthController health = CurrentTarget.GetComponent<HealthController>();

            if (!health.IsDead)
            {
                health.TakeDamage(attackValues.Value);

                E_OnDamageDealt.Invoke(CurrentTarget, attackValues.Value);

                DisplayDamageNumber(CurrentTarget, attackValues.Type, attackValues.Value);

                if (health.IsDead)
                {
                    E_OnEnemyDefeated.Invoke();
                }
            }
        }

        private void DisplayDamageNumber(GameObject targetAttack, DamageType damageType, BigDouble damageDealt)
        {
            var avatar = targetAttack.GetComponentInChildren<UnitAvatar>();

            DamageNumbers.Spawn(avatar.Bounds.RandomPosition(), damageType, damageDealt);
        }

        private MercAttackValues CalculateAttackValue()
        {
            DamageType damageType = DamageType.Normal;

            BigDouble damage = DataValues.DamagePerAttack;

            if (MathsUtlity.PercentChance(App.Bonuses.CriticalHitChance))
            {
                damageType = DamageType.CriticalHit;
                damage *= App.Bonuses.CriticalHitMultiplier;
            }

            return new() { Type = damageType, Value = damage };
        }
    }
}
