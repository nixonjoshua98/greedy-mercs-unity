using GM.Common.Enums;
using GM.Controllers;
using GM.Targets;
using GM.Units;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GM.Mercs.Controllers
{
    public class MercController : GM.Core.GMMonoBehaviour, IMercController
    {
        public MercID Id;
        public UnitAvatar Avatar;

        // = Controllers = //
        IAttackController AttackController;
        IMovementController MoveController;

        Target CurrentTarget;

        // = States = //
        bool IsPaused { get; set; } = false;
        bool IsTargetPriority { get; set; } = false;

        protected TargetList<Target> CurrentTargetList => GameManager.Instance.Enemies;

        void Awake()
        {
            GetComponents();
        }

        void GetComponents()
        {
            AttackController = GetComponent<IAttackController>();
            MoveController = GetComponent<IMovementController>();

            GMLogger.WhenNull(MoveController, "IMovementController is Null");
            GMLogger.WhenNull(AttackController, "IAttackController is Null");
        }

        void FixedUpdate()
        {
            // Pausing the controller generally means the merc is being controlled from outside
            if (IsPaused)
            {

            }

            // Target is marked as a priority ( should find a better name ) which means in this context
            // that the merc will start attacking the target and ignore all position checks
            else if (IsTargetPriority && CanAttackPriorityTarget())
            {
                MoveController.FaceTowards(CurrentTarget.GameObject);

                StartAttack();
            }

            else
            {
                AttackLoop();
            }
        }

        void AttackLoop()
        {
            if (!IsCurrentTargetValid())
            {
                CurrentTarget = GetTargetFromTargetList();
            }
            else
            {
                if (AttackController.IsAvailable)
                {
                    AttackController.MoveTowardsAttackPosition(CurrentTarget);

                    bool inPosition = AttackController.InAttackPosition(CurrentTarget);

                    if (inPosition)
                    {
                        StartAttack();
                    }
                }
            }
        }

        bool CanAttackPriorityTarget() => AttackController.IsAvailable && IsCurrentTargetValid();
        bool IsCurrentTargetValid() => !(CurrentTarget == null || CurrentTarget.GameObject == null || CurrentTarget.Health.IsDead);

        void StartAttack()
        {
            AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
        }


        protected void DealDamageToTarget(Target attackTarget)
        {
            if (attackTarget.GameObject.TryGetComponent(out HealthController health))
            {
                BigDouble dmg = App.Cache.MercDamage(App.Data.Mercs.GetMerc(Id));

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);
            }
        }

        public void Pause()
        {
            IsPaused = true;
            AttackController.Reset();
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void MoveTowards(Vector3 position, Action callback) => MoveController.MoveTowards(position, callback);

        public void SetPriorityTarget(Target target)
        {
            CurrentTarget = target;
            IsTargetPriority = true;

            GMLogger.WhenNull(target.Health, "Fatal Error: Priority target health is Null");

            // Reset the flag once the target has been defeated
            target.Health.E_OnZeroHealth.AddListener(() => { IsTargetPriority = false; });
        }

        Target GetTargetFromTargetList()
        {
            if (CurrentTargetList.Count > 0)
                return CurrentTargetList.MinBy(x => Random.Range(0, 1));

            return null;
        }
    }
}
