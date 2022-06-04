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
        [HideInInspector] public UnityEvent<BigDouble> OnDamageDealt = new();
        [HideInInspector] public UnityEvent E_OnZeroEnergy = new();

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
        private bool IsAttacking => Attack.HasControl;

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
            int idx = SquadController.GetIndex(this);

            if (idx == 0)
            {
                if (!EnemyUnits.Contains(CurrentTarget))
                    EnemyUnits.GetTarget(this, out CurrentTarget);

                if (CanStartAttack())
                {
                    Attack.StartAttack(CurrentTarget.Unit);
                }
                else
                {
                    MoveTowardsCurrentTarget();
                }
            }
            else if (idx > 0)
            {
                FollowUnitInFront(idx);
            }
        }

        protected abstract void MoveTowardsCurrentTarget();

        protected virtual bool CanStartAttack()
        {
            return CurrentTarget != null && Attack.CanStartAttack(CurrentTarget.Unit);
        }

        private void FollowUnitInFront(int idx)
        {
            var unitInFront = SquadController.Get(idx - 1);

            // TODO: 05/06/2022 : Potential code smell
            // Multiple melee units can attack at the same time, and the unit may not be the closest one if attacking.
            // e.g melee unit could be attacking from the right side but we should follow the closest unit on the left side
            if (unitInFront.DataValues.AttackType == UnitAttackType.Melee && unitInFront.IsAttacking)
            {
                var unitsInFront = SquadController.GetUnitsInFront(idx);

                if (unitsInFront.Count > 1)
                {
                    unitInFront = unitsInFront.OrderByDescending(x => Mathf.Abs(x.transform.position.x - transform.position.x)).FirstOrDefault();
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

            health.TakeDamage(attackValues.Value);

            OnDamageDealt.Invoke(attackValues.Value);

            DamageTextPool.Spawn(CurrentTarget.Unit, attackValues.Value, attackValues.Type);
        }

        MercAttackValues CalculateAttackValue()
        {
            DamageType damageType = DamageType.Normal;

            BigDouble damage = DataValues.DamagePerAttack * SetupPayload.EnergyPercentUsedToInstantiate;

            if (SetupPayload.IsEnergyOverload)
                damageType = DamageType.EnergyOvercharge;

            if (MathsUtlity.PercentChance(App.GMCache.CriticalHitChance))
            {
                damageType = DamageType.CriticalHit;
                damage *= App.GMCache.CriticalHitMultiplier;
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
                    IsEnergyDepleted = true;

                    E_OnZeroEnergy.Invoke();

                    EnemyUnits.ReleaseTarget(this, CurrentTarget);

                    this.InvokeAfter(0.25f, () => StartCoroutine(EnergyExhaustedAnimation()));
                }
            }
        }
    }
}
