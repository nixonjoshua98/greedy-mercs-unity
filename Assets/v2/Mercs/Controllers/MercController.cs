using GM.Common.Enums;
using GM.DamageTextPool;
using GM.Units;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{
    public class MercController : GM.Mercs.MercBaseClass
    {
        [Header("Components")]
        [SerializeField] MovementController Movement;
        [SerializeField] AttackController AttackController;

        // = Controllers = //
        UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Managers
        IEnemyUnitFactory UnitManager;
        IDamageTextPool DamageTextPool;
        GameManager GameManager;
        ISquadController SquadController;

        // Energy
        bool IsEnergyDepleted;
        float EnergyRemaining;

        // ...
        GM.Mercs.Data.AggregatedMercData MercDataValues => App.GMData.Mercs.GetMerc(Id);


        void Awake()
        {
            GetRequiredComponents();
            SubscribeToEvents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        void SubscribeToEvents()
        {
            AttackController.E_AttackFinished.AddListener(AttackController_OnAttackFinished);
        }

        protected void GetRequiredComponents()
        {
            DamageTextPool = this.GetComponentInScene<IDamageTextPool>();
            UnitManager = this.GetComponentInScene<IEnemyUnitFactory>();
            GameManager = this.GetComponentInScene<GameManager>();
            SquadController = this.GetComponentInScene<ISquadController>();
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
            DamageType damageType = DamageType.Normal;

            BigDouble damage = MercDataValues.DamagePerAttack;

            // Energy overcharge
            if (SetupPayload.EnergyPercentUsedToInstantiate > 1.0f)
            {
                // Only set this damage type for 150%+ otherwise it will near always be energy overcharge
                if (SetupPayload.EnergyPercentUsedToInstantiate > 1.5)
                    damageType = DamageType.EnergyOvercharge;

                damage *= SetupPayload.EnergyPercentUsedToInstantiate;
            }

            // Critical hit
            if (MathUtils.PercentChance(App.GMCache.CriticalHitChance))
            {
                damageType = DamageType.CriticalHit;
                damage *= App.GMCache.CriticalHitMultiplier;
            }

            if (GameManager.DealDamageToTarget(damage, false))
            {
                DamageTextPool.Spawn(CurrentTarget, damage, damageType);

                OnDamageDealt.Invoke(damage);
            }
        }

        IEnumerator EnergyExhaustedAnimation()
        {
            Vector3 originalScale = transform.localScale;

            // Scale down the unit eventually to zero
            Enumerators.Lerp01(this, 2, (value) => { transform.localScale = originalScale * (1 - value); });

            // Move down slightly
            yield return Movement.MoveTowardsEnumerator(transform.position - new Vector3(0, 1.5f));

            // Move left until out of camera view
            yield return Enumerators.InvokeUntil(() => !Camera.main.IsVisible(Avatar.Bounds.max), () => Movement.MoveDirection(Vector2.left));

            Destroy(gameObject);
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

                StartCoroutine(EnergyExhaustedAnimation());
            }
        }
    }
}
