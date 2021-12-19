using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Units;
using GM.Controllers;
using GM.Common.Enums;
using System;
using System.Linq;
using GM.Targets;

namespace GM.Mercs.Controllers
{
    public class MercController : GM.Core.GMMonoBehaviour, IMercController
    {
        public MercID Id;
        public UnitAvatar Avatar;


        public IAttackController AttackController;
        public IMovementController MoveController;


        private Target CurrentTarget;

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
                if (!AttackController.IsAttacking())
                {
                    MoveTowardsAttackPosition();

                    if (CanAttackTarget())
                    {
                        StartAttack();
                    }
                }
            }
        }

        bool CanAttackPriorityTarget()
        {
            return !AttackController.IsAttacking() && IsCurrentTargetValid();
        }

        void MoveTowardsAttackPosition()
        {
            AttackController.MoveTowardsAttackPosition(CurrentTarget);
        }

        bool CanAttackTarget()
        {
            return AttackController.InAttackPosition(CurrentTarget);
        }

        void StartAttack()
        {
            AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
        }


        protected void DealDamageToTarget()
        {
            if (CurrentTarget.GameObject.TryGetComponent(out HealthController health))
            {
                BigDouble dmg = App.Cache.MercDamage(App.Data.Mercs.GetMerc(Id));

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);
            }
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Move(Vector3 position, Action callback)
        {
            MoveController.MoveTowards(position, callback);
        }

        public void SetPriorityTarget(Target target)
        {
            CurrentTarget = target;
            IsTargetPriority = true;

            GMLogger.WhenNull(target.Health, "Priority merc target health is Null");

            target.Health.E_OnZeroHealth.AddListener(() => { IsTargetPriority = false; });
        }

        protected bool IsCurrentTargetValid()
        {
            return !(CurrentTarget == null || CurrentTarget.GameObject == null || CurrentTarget.Health.IsDead);
        }

        protected Target GetTargetFromTargetList()
        {
            if (CurrentTargetList.Count > 0)
                return CurrentTargetList.MinBy(x => x.Position.x);

            return null;
        }
    }
}
