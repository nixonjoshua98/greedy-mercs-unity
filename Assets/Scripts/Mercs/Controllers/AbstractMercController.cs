using GM.Common;
using GM.Common.Enums;
using GM.Controllers;
using GM.DamageTextPool;
using GM.Units;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    record MercAttackValues
    {
        public DamageType Type;
        public BigDouble Value;
    }
}


namespace GM.Mercs.Controllers
{
    public abstract class AbstractMercController : UnitBase
    {
        public MercID ID = MercID.UNKNOWN;

        [Header("Components")]
        [SerializeField] protected MovementController Movement;
        [SerializeField] private AbstractAttackController Attack;

        [Header("Events")]
        [HideInInspector] public UnityEvent<BigDouble> E_OnDamageDealt = new();
        [HideInInspector] public UnityEvent E_OnZeroEnergy { get; set; } = new();
        [HideInInspector] public UnityEvent E_OnEnemyDefeated = new();

        // Scene instances
        private IDamageTextPool DamageTextPool;

        // Current attack target which the unit should focus
        protected AttackTarget CurrentTarget;

        /* Init Values */
        protected EnemyUnitCollection EnemyUnits;
        protected MercSquadController SquadController;

        // Energy
        private bool IsEnergyDepleted;
        private float EnergyRemaining;

        protected MercSetupPayload SetupPayload;

        /* Value Forwarding */
        private bool HasTarget => CurrentTarget is not null;

        /* Game Values */
        public GM.Mercs.Data.AggregatedMercData DataValues => App.Mercs.GetMerc(ID);

        public void Init(MercSetupPayload payload, MercSquadController squad, EnemyUnitCollection enemyUnits)
        {
            SetupPayload = payload;
            EnemyUnits = enemyUnits;
            SquadController = squad;
        }

        private void Awake()
        {
            GetRequiredComponents();

            EnergyRemaining = DataValues.BattleEnergyCapacity;
        }

        protected void GetRequiredComponents()
        {
            DamageTextPool = this.GetComponentInScene<IDamageTextPool>();
        }

        private void FixedUpdate()
        {
            if (!IsEnergyDepleted && !Attack.HasControl)
            {
                _UpdateMerc();
            }
        }

        private void _UpdateMerc()
        {
            bool hasTarget = false;

            if (CanFetchTarget())
                hasTarget = FetchTarget();

            if (hasTarget)
            {
                if (CanStartAttack())
                    Attack.StartAttack(CurrentTarget.Unit);

                else
                    MoveTowardsCurrentTarget();
            }
            else
            {
                FollowUnitInFront();
            }
        }

        protected virtual bool FetchTarget()
        {
            bool hasTarget = true;

            if (!EnemyUnits.Contains(CurrentTarget))
                hasTarget = EnemyUnits.GetTarget(this, out CurrentTarget);

            return hasTarget;
        }

        protected abstract bool CanFetchTarget();
        protected abstract void MoveTowardsCurrentTarget();

        protected virtual bool CanStartAttack()
        {
            return CurrentTarget != null && Attack.CanStartAttack;
        }

        private void FollowUnitInFront()
        {
            int idx = SquadController.GetIndex(this);

            var unitInFront = SquadController.Get(idx - 1);

            if (unitInFront.DataValues.AttackType == UnitAttackType.Melee && unitInFront.HasTarget)
            {
                if (unitInFront.CurrentTarget.TryGetMeleeAttacker(AttackSide.Left, out var leftAttacker))
                {
                    unitInFront = leftAttacker;
                }
            }

            Vector3 position = new(unitInFront.Avatar.Bounds.min.x - (unitInFront.Avatar.Bounds.size.x * 0.75f), transform.position.y);

            MoveToPosition(position);
        }

        protected void MoveToPosition(Vector3 vec)
        {
            if (transform.position != vec)
            {
                Movement.MoveTowards(vec);
            }
            else
            {
                Avatar.PlayAnimation(Avatar.Animations.Idle);
            }
        }

        public void DealDamageToTarget()
        {
            var attackValues = CalculateAttackValue();

            ReduceEnergy(DataValues.EnergyConsumedPerAttack);

            HealthController health = CurrentTarget.Unit.GetComponent<HealthController>();

            if (health.CanTakeDamage)
            {
                health.TakeDamage(attackValues.Value);

                E_OnDamageDealt.Invoke(attackValues.Value);

                DamageTextPool.Spawn(CurrentTarget.Unit, attackValues.Value, attackValues.Type);

                if (health.IsDead)
                {
                    E_OnEnemyDefeated.Invoke();
                }
            }
        }

        MercAttackValues CalculateAttackValue()
        {
            DamageType damageType = DamageType.Normal;

            BigDouble damage = DataValues.DamagePerAttack * SetupPayload.RechargePercentage;

            if (SetupPayload.IsOverCharge)
                damageType = DamageType.Overcharge;

            if (MathsUtlity.PercentChance(App.Values.CriticalHitChance))
            {
                damageType = DamageType.CriticalHit;
                damage *= App.Values.CriticalHitMultiplier;
            }

            return new() { Type = damageType, Value = damage };
        }

        private IEnumerator EnergyExhaustedAnimation()
        {
            Vector3 originalScale = transform.localScale;

            this.Lerp01(3, (value) => { transform.localScale = originalScale * (1 - (value * 0.5f)); });

            // Move down slightly
            yield return Movement.MoveTowardsEnumerator(transform.position - new Vector3(0, 1.5f));

            // Move left until out of camera view
            yield return Enumerators.InvokeUntil(() => !Camera.main.IsVisible(Avatar.Bounds.max), () => Movement.MoveDirection(Vector2.left));

            Destroy(gameObject);
        }

        private void ReduceEnergy(float value)
        {
            if (EnergyRemaining > 0)
            {
                EnergyRemaining -= value;

                if (EnergyRemaining <= 0)
                {
                    Attack.Stop();

                    IsEnergyDepleted = true;

                    E_OnZeroEnergy.Invoke();

                    EnemyUnits.ReleaseTarget(this, CurrentTarget);

                    this.InvokeAfter(0.25f, () => StartCoroutine(EnergyExhaustedAnimation()));
                }
            }
        }
    }
}
