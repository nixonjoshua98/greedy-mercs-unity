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

        public void Move(Vector3 position, Action callback)
        {
            transform.position = position;

            callback.Invoke();
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
