using GM.Common;
using GM.Common.Enums;
using GM.Controllers;
using GM.DamageTextPool;
using GM.Units;
using System.Collections;
using System.Collections.Generic;
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

    public class MercController : MercBaseClass
    {
        [Header("Components")]
        [SerializeField] private MovementController Movement;

        private List<IUnitActionController> ActionControllers;
        private UnitBase CurrentTarget;

        [Header("Events")]
        public UnityEvent<BigDouble> OnDamageDealt = new();

        // Scene instances
        private IDamageTextPool DamageTextPool;
        private ISquadController SquadController;
        private IEnemyUnitCollection EnemyUnits;

        // Energy
        private bool IsEnergyDepleted;
        private float EnergyRemaining;

        // Properties
        private bool HasControl => !ActionControllers.Any(x => x.HasControl);

        // ...
        public GM.Mercs.Data.AggregatedMercData MercDataValues => App.Mercs.GetMerc(Id);

        private void Awake()
        {
            GetRequiredComponents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        protected void GetRequiredComponents()
        {
            ActionControllers = GetComponents<IUnitActionController>().OrderByDescending(x => x.Priority).ToList();
            EnemyUnits = this.GetComponentInScene<IEnemyUnitCollection>();
            DamageTextPool = this.GetComponentInScene<IDamageTextPool>();
            SquadController = this.GetComponentInScene<ISquadController>();
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
            int idx = SquadController.GetQueuePosition(this);

            if (idx == 0)
            {
                ProcessActions();

                if (!HasControl)
                {
                    return;
                }

                Movement.MoveDirection(Vector2.right);
            }

            else
            {
                FollowUnitInFront(idx);
            }
        }

        private void ProcessActions()
        {
            for (int i = 0; i < ActionControllers.Count; i++)
            {
                IUnitActionController action = ActionControllers[i];

                if (!HasControl)
                    return;

                if (action.WantsControl())
                {
                    action.GiveControl();
                }
            }
        }


        /// <summary>
        /// Move forward in the merc queue
        /// </summary>
        private void FollowUnitInFront(int queueIndex)
        {
            UnitBase unit = SquadController.GetUnitAtQueuePosition(queueIndex - 1);

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

        public void DealDamageToTarget(UnitBase target)
        {
            CurrentTarget = target;

            ReduceEnergy(MercDataValues.EnergyConsumedPerAttack);

            DealDamageToTarget();
        }

        private void DealDamageToTarget()
        {
            var attackValues = CalculateAttackValue();

            // Target is invalid at this point
            if (!EnemyUnits.ContainsUnit(CurrentTarget))
                return;

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

            if (Utility.Maths.PercentChance(App.GMCache.CriticalHitChance))
            {
                damageType = DamageType.CriticalHit;
                damage *= App.GMCache.CriticalHitMultiplier;
            }

            return new() { Type = damageType, Value = damage };
        }


        private IEnumerator EnergyExhaustedAnimation()
        {
            Vector3 originalScale = transform.localScale;

            // Scale down the unit eventually to zero
            Enumerators.Lerp01(this, 3, (value) => { transform.localScale = originalScale * (1 - (value * 0.5f)); });

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

                    SquadController.RemoveFromQueue(this);

                    for (int i = 0; i < ActionControllers.Count; i++)
                    {
                        var action = ActionControllers[i];

                        if (action.HasControl)
                        {
                            action.RemoveControl();
                        }
                    }

                    Enumerators.InvokeAfter(this, 0.25f, () => StartCoroutine(EnergyExhaustedAnimation()));
                }
            }
        }
    }
}
