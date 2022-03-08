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
        [SerializeField] AttackController Attack;
        /* Optional */ ISpecialAttackController SpecialAttack;

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

        // Properties
        public bool HasEnergy => EnergyRemaining > 0 && !IsEnergyDepleted;
        bool HasSpecialAttack => SpecialAttack != null;

        // ...
        public GM.Mercs.Data.AggregatedMercData MercDataValues => App.GMData.Mercs.GetMerc(Id);

        void Awake()
        {
            GetRequiredComponents();
            SubscribeToEvents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        void SubscribeToEvents()
        {
            Attack.E_AttackFinished.AddListener(AttackController_OnAttackFinished);
        }

        protected void GetRequiredComponents()
        {
            SpecialAttack = GetCachedComponent<ISpecialAttackController>();

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
                if (HasSpecialAttack)
                {
                    // Give control to the special attack if it wants it
                    if (CanGiveControlToSpecialAttack())
                        SpecialAttack.GiveControl();

                    // Return out the loop if we do not have control
                    if (SpecialAttack.HasControl)
                        return;
                }
               
                if (TryGetValidTarget(ref CurrentTarget))
                {
                    // We are not attacking and not in attack distance so move towards the target
                    if (!Attack.IsAttacking && !Attack.IsWithinAttackDistance(CurrentTarget))
                    {
                        Attack.MoveTowardsTarget(CurrentTarget);
                    }

                    // Start an attack (assuming we can)
                    else if (Attack.CanStartAttack(CurrentTarget))
                    {
                        StartAttack();
                    }

                    else if (Attack.IsOnCooldown)
                    {
                        Avatar.PlayAnimation(Avatar.Animations.Idle);
                    }
                }
            }

            else
            {
                FollowUnitInFront(idx);
            }
        }


        bool CanGiveControlToSpecialAttack() => !Attack.IsAttacking && !SpecialAttack.HasControl && SpecialAttack.WantsControl();

        /// <summary>
        /// Move forward in the merc queue
        /// </summary>
        void FollowUnitInFront(int queueIndex)
        {
            UnitBaseClass unit = SquadController.GetUnitAtQueuePosition(queueIndex - 1);

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

        protected void StartAttack()
        {
            Attack.StartAttack(CurrentTarget, DealDamageToTarget);
        }

        public void PerformAttack(UnitBaseClass target)
        {
            CurrentTarget = target;

            DealDamageToTarget();
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

        public bool TryGetValidTarget(ref UnitBaseClass current)
        {
            if (!UnitManager.ContainsEnemyUnit(current))
                UnitManager.TryGetEnemyUnit(out current);

            return UnitManager.ContainsEnemyUnit(current);
        }

        IEnumerator EnergyExhaustedAnimation()
        {
            Vector3 originalScale = transform.localScale;

            // Scale down the unit eventually to zero
            Enumerators.Lerp01(this, 3, (value) => { transform.localScale = originalScale * (1 - value); });

            // Move down slightly
            yield return Movement.MoveTowardsEnumerator(transform.position - new Vector3(0, 1.5f));

            // Move left until out of camera view
            yield return Enumerators.InvokeUntil(() => !Camera.main.IsVisible(Avatar.Bounds.max), () => Movement.MoveDirection(Vector2.left));

            Destroy(gameObject);
        }

        public void ReduceEnergy(float value)
        {
            if (EnergyRemaining > 0)
            {
                EnergyRemaining -= value;

                if (EnergyRemaining <= 0)
                {
                    IsEnergyDepleted = true;

                    SquadController.RemoveMercFromQueue(this);

                    StartCoroutine(EnergyExhaustedAnimation());
                }
            }
        }

        // = Callbacks = //

        protected void AttackController_OnAttackImpact()
        {
            DealDamageToTarget();
        }

        void AttackController_OnAttackFinished()
        {
            ReduceEnergy(MercDataValues.EnergyConsumedPerAttack);
        }
    }
}
