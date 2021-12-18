using GM.Targets;
using System;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;
using MercID = GM.Common.Enums.MercID;

namespace GM.Units
{
    public class MercController : Core.GMMonoBehaviour
    {
        public Target CurrentTarget;
        protected TargetList<Target> CurrentTargetList => GameManager.Instance.Enemies;

        public MercID ID { get; private set; } = MercID.NONE;

        [Header("References")]
        [SerializeField] GameObject AttackImpactPS;

        [Header("Components")]
        public Animator anim;
      
        public IMovementController Movement { get; private set; }
        public UnitAttack Attack { get; private set; }

        void Start()
        {
            GetComponents();
            SubscribeToEvents();
            GetInitialTarget();
        }

        public void Setup(MercID _mercId)
        {
            ID = _mercId;
        }


        // Attempt to grab a priority target. Useful when the Merc is hot-swapped
        // and it has missed a Boss spawn event.
        void GetInitialTarget()
        {
            TryGetBossFromTargetList(ref CurrentTarget);
        }


        void SubscribeToEvents()
        {
            Attack.E_OnAttackImpact.AddListener(OnAttackImpact);

            GameManager.Instance.E_BossSpawn.AddListener(boss => {
                Attack.Disable();

                CurrentTarget = boss;
            });
        }

        void GetComponents()
        {
            Movement = GetComponent<IMovementController>();
            Attack = GetComponent<UnitAttack>();
        }

        void FixedUpdate()
        {
            if (IsTargetValid(CurrentTarget))
            {
                switch (CurrentTarget.Type)
                {
                    case TargetType.WaveEnemy:
                        Attack.Process(CurrentTarget.GameObject);
                        break;

                    case TargetType.Boss:
                        Attack.DirtyAttack(CurrentTarget.GameObject);
                        break;
                }
            }
            else
            {
                CurrentTarget = GetTargetFromTargetList();

                if (!Attack.IsAttacking)
                {
                    Movement.MoveDirection(Vector2.right);
                }
            }
        }

        public void Move(Vector3 position, Action action)
        {
            Attack.Disable();

            Movement.MoveTowards(position, () =>
            {
                action.Invoke();
            });
        }

        void OnAttackImpact(GameObject target)
        {
            if (target != null && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = App.Cache.MercDamage(App.Data.Mercs.GetMerc(ID));

                App.Cache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);

                PostAttackImpact(target);
            }
        }

        protected virtual void PostAttackImpact(GameObject target)
        {
            InstantiateAttackImpactPS(target);
        }


        
        void InstantiateAttackImpactPS(GameObject target)
        {
            if (AttackImpactPS != null && target.TryGetComponentInChildren(out UnitAvatar avatar))
            {
                Instantiate(AttackImpactPS, avatar.AvatarCenter, Quaternion.identity, null);
            }
        }

        protected Target GetTargetFromTargetList()
        {
            if (CurrentTargetList.Count > 0)
            {
                return CurrentTargetList[UnityEngine.Random.Range(0, CurrentTargetList.Count)];
            }

            return null;
        }

        protected bool IsTargetValid(Target target)
        {
            if (target == null || target.GameObject == null)
            {
                return false;
            }
            else if (target.Health.IsDead)
            {
                return false;
            }

            return true;
        }

        protected bool TryGetBossFromTargetList(ref Target boss)
        {
            return CurrentTargetList.TryGetWithType(TargetType.Boss, ref boss);
        }
    }
}