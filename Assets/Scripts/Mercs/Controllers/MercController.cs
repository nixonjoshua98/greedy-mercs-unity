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

        private UnitBase CurrentTarget;

        [Header("Events")]
        [HideInInspector] public UnityEvent<BigDouble> OnDamageDealt = new();

        // Scene instances
        private IDamageTextPool DamageTextPool;
        private MercSquadController SquadController;
        private EnemyUnitCollection EnemyUnits;

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
            if (!IsEnergyDepleted)
            {
                UpdateMercWithEnergy();
            }
        }

        private void UpdateMercWithEnergy()
        {
            if (Attack.HasControl)
                return;

            EnemyUnits.TryGet(ref CurrentTarget);

            if (Attack.CanStartAttack(CurrentTarget))
            {
                Attack.StartAttack(CurrentTarget);
            }

            else
            {
                int idx = SquadController.GetIndex(this);

                if (idx == 0)
                    Movement.MoveDirection(Vector2.right);

                else
                    FollowUnitInFront(idx);
            }
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
            if (!EnemyUnits.Contains(CurrentTarget))
                return;

            var attackValues = CalculateAttackValue();

            ReduceEnergy(MercDataValues.EnergyConsumedPerAttack);

            HealthController health = CurrentTarget.GetComponent<HealthController>();

            health.TakeDamage(attackValues.Value);

            OnDamageDealt.Invoke(attackValues.Value);

            DamageTextPool.Spawn(CurrentTarget, attackValues.Value, attackValues.Type);
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

                    this.InvokeAfter(0.25f, () => StartCoroutine(EnergyExhaustedAnimation()));
                }
            }
        }
    }
}
