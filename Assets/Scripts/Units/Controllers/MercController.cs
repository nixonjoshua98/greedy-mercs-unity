using GM.Targets;
using System;
using UnityEngine;
using HealthController = GM.Controllers.HealthController;
using MercID = GM.Common.Enums.MercID;

namespace GM.Units
{
    public class MercController : UnitController
    {
        public Target CurrentTarget;
        protected override TargetList<Target> CurrentTargetList => GameManager.Instance.Enemies;

        public MercID ID { get; private set; } = MercID.NONE;

        [Header("References")]
        [SerializeField] GameObject AttackImpactPS;

        [Header("Components")]
        public Animator anim;
      
        public UnitMovement Movement { get; private set; }
        public UnitAttack Attack { get; private set; }

        void Start()
        {
            GetComponents();
            SubscribeToEvents();
            GetInitialTarget();
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
            Movement = GetComponent<UnitMovement>();
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

        // = = = Public = = = //

        public void Setup(MercID _mercId)
        {
            ID = _mercId;
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

                InstantiateAttackImpactPS(target);
            }
        }

        protected override Target GetTargetFromTargetList()
        {
            if (CurrentTargetList.Count > 0)
            {
                return CurrentTargetList[UnityEngine.Random.Range(0, CurrentTargetList.Count)];
            }

            return null;
        }

        void InstantiateAttackImpactPS(GameObject target)
        {
            if (target != null && AttackImpactPS != null && target.TryGetComponentInChildren(out UnitAvatar avatar))
            {
                Instantiate(AttackImpactPS, avatar.AvatarCenter, Quaternion.identity, null);
            }
        }
    }
}