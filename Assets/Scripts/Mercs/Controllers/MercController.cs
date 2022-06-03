using GM.Common;
using GM.Common.Enums;
using GM.Controllers;
using GM.DamageTextPool;
using GM.Units;
using System.Collections;
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
    public class MercController : MercBase
    {
        [Header("Components")]
        [SerializeField] MovementController Movement;
        [SerializeField] AttackController Attack;

        private AttackTarget CurrentTarget;

        [Header("Events")]
        [HideInInspector] public UnityEvent<BigDouble> OnDamageDealt = new();

        // Scene instances
        private IDamageTextPool DamageTextPool;
        private MercSquadController SquadController;
        public EnemyUnitCollection EnemyUnits;

        // Energy
        private bool IsEnergyDepleted;
        private float EnergyRemaining;

        protected MercSetupPayload SetupPayload;

        // ...
        public GM.Mercs.Data.AggregatedMercData MercDataValues => App.Mercs.GetMerc(Id);

        public void Init(MercSetupPayload payload, EnemyUnitCollection enemyUnits)
        {
            SetupPayload = payload;
            EnemyUnits = enemyUnits;
        }

        private void Awake()
        {
            GetRequiredComponents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        protected void GetRequiredComponents()
        {
            DamageTextPool = this.GetComponentInScene<IDamageTextPool>();
            SquadController = this.GetComponentInScene<MercSquadController>();
        }

        private void FixedUpdate()
        {
            if (!IsEnergyDepleted && !Attack.HasControl)
            {
                UpdateMercWithEnergy();
            }
        }

        private void UpdateMercWithEnergy()
        {
            int idx = SquadController.GetIndex(this);

            if (idx == 0)
            {
                if (!EnemyUnits.Contains(CurrentTarget))
                    EnemyUnits.GetTarget(this, out CurrentTarget);

                if (Attack.CanStartAttack(CurrentTarget.Unit))
                    Attack.StartAttack(CurrentTarget.Unit);

                else
                    Movement.MoveDirection(Vector2.right);
            }

            else
                FollowUnitInFront(idx);
        }

        private void FollowUnitInFront(int queueIndex)
        {
            UnitBase unit = SquadController.Get(queueIndex - 1);

            Vector3 targetPosition = new(unit.Avatar.Bounds.min.x - unit.Avatar.Bounds.size.x, transform.position.y);

            if (transform.position != targetPosition)
            {
                Movement.MoveTowards(targetPosition);
            }
            else
            {
                Avatar.PlayAnimation(Avatar.Animations.Idle);
            }
        }

        public void DealDamageToTarget()
        {
            var attackValues = CalculateAttackValue();

            ReduceEnergy(MercDataValues.EnergyConsumedPerAttack);

            HealthController health = CurrentTarget.Unit.GetComponent<HealthController>();

            health.TakeDamage(attackValues.Value);

            OnDamageDealt.Invoke(attackValues.Value);

            DamageTextPool.Spawn(CurrentTarget.Unit, attackValues.Value, attackValues.Type);
        }

        MercAttackValues CalculateAttackValue()
        {
            DamageType damageType = DamageType.Normal;

            BigDouble damage = MercDataValues.DamagePerAttack * SetupPayload.EnergyPercentUsedToInstantiate;

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

                    SquadController.Remove(this);

                    EnemyUnits.ReleaseTarget(this, CurrentTarget);

                    this.InvokeAfter(0.25f, () => StartCoroutine(EnergyExhaustedAnimation()));
                }
            }
        }
    }
}
