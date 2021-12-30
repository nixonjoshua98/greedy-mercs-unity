using GM.Common.Enums;
using GM.Controllers;
using GM.Targets;
using UnityEngine.Events;
using GM.Units;
using Random = UnityEngine.Random;

namespace GM.Mercs.Controllers
{
    public interface IMercController : IUnitController
    {
        UnityEvent<BigDouble> OnDamageDealt { get; set; }

        void Pause();
        void Resume();
        void SetPriorityTarget(Target target);
    }

    public class MercController : UnitController, IMercController
    {
        public MercID Id;
        public UnitAvatar Avatar;

        // = Controllers = //
        IAttackController AttackController;

        Target CurrentTarget;

        // = States = //
        bool IsPaused { get; set; } = false;
        bool IsTargetPriority { get; set; } = false;

        // = Events = //
        public UnityEvent<BigDouble> OnDamageDealt { get; set; } = new UnityEvent<BigDouble>();

        protected TargetList<UnitTarget> CurrentTargetList => GameManager.Instance.Enemies;

        void Awake()
        {
            GetComponents();
        }

        protected override void GetComponents()
        {
            base.GetComponents();

            AttackController = GetComponent<IAttackController>();

            GMLogger.WhenNull(Movement, "IMovementController is Null");
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
            else if (IsTargetPriority && CanAttackPriorityTarget)
            {
                Movement.LookAt(CurrentTarget.Position);

                StartAttack();
            }

            else
            {
                AttackLoop();
            }
        }

        void AttackLoop()
        {
            if (!IsCurrentTargetValid)
            {
                CurrentTarget = GetTargetFromTargetList();
            }
            else
            {
                if (AttackController.IsAvailable)
                {
                    if (!AttackController.InAttackPosition(CurrentTarget))
                    {
                        AttackController.MoveTowardsAttackPosition(CurrentTarget);
                    }
                    else
                    {
                        StartAttack();
                    }
                }
            }
        }

        bool CanAttackPriorityTarget => AttackController.IsAvailable && IsCurrentTargetValid;
        bool IsCurrentTargetValid => !(CurrentTarget == null || CurrentTarget.GameObject == null || CurrentTarget.Health.IsDead);

        void StartAttack()
        {
            AttackController.StartAttack(CurrentTarget, DealDamageToTarget);
        }


        protected void DealDamageToTarget(Target attackTarget)
        {
            if (attackTarget.GameObject.TryGetComponent(out HealthController health))
            {
                BigDouble dmg = App.Data.Mercs.GetMerc(Id).DamagePerAttack;

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);

                OnDamageDealt.Invoke(dmg);
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

        public void Idle()
        {
            Avatar.PlayAnimation(Avatar.AnimationStrings.Idle);
        }

        public void SetPriorityTarget(Target target)
        {
            CurrentTarget = target;
            IsTargetPriority = true;

            GMLogger.WhenNull(target.Health, "Fatal Error: Priority target health is Null");

            // Reset the flag once the target has been defeated
            target.Health.OnZeroHealth.AddListener(() => { IsTargetPriority = false; });
        }

        UnitTarget GetTargetFromTargetList()
        {
            if (CurrentTargetList.Count > 0)
                return CurrentTargetList.MinBy(x => Random.Range(0, 1));

            return null;
        }
    }
}
