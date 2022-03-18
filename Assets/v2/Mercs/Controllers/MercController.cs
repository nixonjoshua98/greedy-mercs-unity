using GM.Common.Enums;
using GM.DamageTextPool;
using GM.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs.Controllers
{

    public class MercController : MercBaseClass
    {
        [Header("Components")]
        [SerializeField] MovementController Movement;
        List<IUnitActionController> ActionControllers;

        UnitBaseClass CurrentTarget;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        // Managers
        IDamageTextPool DamageTextPool;
        GameManager GameManager;
        ISquadController SquadController;

        // Energy
        bool IsEnergyDepleted;
        float EnergyRemaining;

        // Properties
        bool HasControl => !ActionControllers.Any(x => x.HasControl);

        // ...
        public GM.Mercs.Data.AggregatedMercData MercDataValues => App.Mercs.GetMerc(Id);

        void Awake()
        {
            GetRequiredComponents();

            EnergyRemaining = MercDataValues.BattleEnergyCapacity;
        }

        protected void GetRequiredComponents()
        {
            ActionControllers = GetComponents<IUnitActionController>().OrderByDescending(x => x.Priority).ToList();

            DamageTextPool = this.GetComponentInScene<IDamageTextPool>();
            GameManager = this.GetComponentInScene<GameManager>();
            SquadController = this.GetComponentInScene<ISquadController>();
        }

        void FixedUpdate()
        {
            if (!IsEnergyDepleted)
            {
                UpdateMercWithEnergy();
            }
        }

        void UpdateMercWithEnergy()
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


        void ProcessActions()
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

        public void DealDamageToTarget(UnitBaseClass target)
        {
            CurrentTarget = target;

            ReduceEnergy(MercDataValues.EnergyConsumedPerAttack);

            DealDamageToTarget();
        }

        void DealDamageToTarget()
        {
            DamageType damageType = DamageType.Normal;

            BigDouble damage = MercDataValues.DamagePerAttack;

            if (SetupPayload.EnergyPercentUsedToInstantiate == 2.0f)
                damageType = DamageType.EnergyOvercharge;

            damage *= SetupPayload.EnergyPercentUsedToInstantiate;

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
            Enumerators.Lerp01(this, 3, (value) => { transform.localScale = originalScale * (1 - (value * 0.5f)); });

            // Move down slightly
            yield return Movement.MoveTowardsEnumerator(transform.position - new Vector3(0, 1.5f));

            // Move left until out of camera view
            yield return Enumerators.InvokeUntil(() => !Camera.main.IsVisible(Avatar.Bounds.max), () => Movement.MoveDirection(Vector2.left));

            Destroy(gameObject);
        }

        void ReduceEnergy(float value)
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
