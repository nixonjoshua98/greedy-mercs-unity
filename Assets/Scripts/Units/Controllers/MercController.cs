using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class MercController : UnitController
    {
        public MercID ID { get; private set; } = MercID.NONE;

        [Header("Components")]
        public Animator anim;

        [Space]

        public UnitMovement movement;
        public UnitAttack attack;

        GameObject CurrentTarget;

        bool _setupCalled = false;

        void Start()
        {
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
            attack.E_OnAttackImpact.AddListener(OnAttackImpact);

            GameManager.Get.E_OnBossSpawn.AddListener(boss => {
                attack.Stop();

                CurrentTarget = boss;
            });
        }


        void FixedUpdate()
        {
            switch (CurrentTarget == null ? "..." : CurrentTarget.tag)
            {
                case Tags.Enemy:
                    attack.Process(CurrentTarget);
                    break;

                case Tags.Boss:
                    attack.Process(CurrentTarget);
                    break;

                default:
                    WhileMissingTarget();
                    break;
            }
        }


        void WhileMissingTarget()
        {
            if (!attack.IsAttacking)
            {
                CurrentTarget = GetTarget();

                movement.MoveDirection(Vector2.right);
            }
        }

        // = = = Public = = = //

        public void Setup(MercID _mercId)
        {
            _setupCalled = true;

            ID = _mercId;
        }

        // = = = Callbacks/Events = = = //

        void OnAttackImpact(GameObject target)
        {
            if (target && target.TryGetComponent(out HealthController hp))
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