using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MercID = GM.Common.Enums.MercID;

using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public class MercController : UnitController
    {
        public MercID ID { get; private set; } = MercID.NONE;

        [Header("Components")]
        public Animator anim;
        
        public UnitMovement Movement { get; private set; }
        public UnitAttack Attack{ get; private set; }

        GameObject CurrentTarget;

        bool _setupCalled = false;

        void Start()
        {
            GetComponents();
            SubscribeToEvents();
            GetInitialTarget();

            if (!_setupCalled)
                Debug.LogError($"Setup not called on {name}");
        }


        // Attempt to grab a priority target. Useful when the Merc is hot-swapped
        // and it has missed a Boss spawn event.
        void GetInitialTarget()
        {
            if (GameManager.Get.TryGetBoss(out GameObject boss))
            {
                CurrentTarget = boss;
            }
        }


        void SubscribeToEvents()
        {
            Attack.E_OnAttackImpact.AddListener(OnAttackImpact);

            GameManager.Get.E_BossSpawn.AddListener(boss => {
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
            switch (CurrentTarget == null ? "..." : CurrentTarget.tag)
            {
                case Tags.Enemy:
                    Attack.Process(CurrentTarget);
                    break;

                case Tags.Boss:
                    Attack.DirtyAttack(CurrentTarget);
                    break;

                default:
                    WhileMissingTarget();
                    break;
            }
        }


        void WhileMissingTarget()
        {
            if (!Attack.IsAttacking)
            {
                CurrentTarget = GetTarget();

                Movement.MoveDirection(Vector2.right);
            }
        }

        // = = = Public = = = //

        public void Setup(MercID _mercId)
        {
            _setupCalled = true;

            ID = _mercId;
        }

        public void PriorityMove(Vector3 position, UnityAction<MercController> action)
        {
            Attack.Disable();

            Movement.MoveTowards(position, () =>
            {
                action.Invoke(this);
            });
        }

        // = = = Callbacks/Events = = = //

        void OnAttackImpact(GameObject target)
        {
            if (target != null && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(ID);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }

        // = = = ^
        GameObject GetTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            return targets.Length == 0 ? null : targets[Random.Range(0, targets.Length)];
        }
    }
}