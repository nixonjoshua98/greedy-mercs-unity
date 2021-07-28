using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{

    public class MercController : UnitController
    {
        [SerializeField] MercID _MercID;

        public MercID ID { get { return _MercID; } }

        [Header("Components")]
        public Animator anim;

        [Space]

        public UnitMovement movement;
        public UnitAttack attack;

        GameObject currentTarget;

        bool _setupCalled = false;

        void Start()
        {
            SubscribeToEvents();

            if (!_setupCalled)
            {
                Debug.LogError($"Setup not called on {name}");
            }
        }


        void SubscribeToEvents()
        {
            attack.E_OnAttackImpact.AddListener(OnAttackImpact);
        }


        void FixedUpdate()
        {
            switch (currentTarget == null ? "..." : currentTarget.tag)
            {
                case Tags.Enemy:
                    attack.Process(currentTarget);
                    break;

                case Tags.Boss:
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
                currentTarget = GetTarget();

                movement.MoveDirection(Vector2.right);
            }
        }

        // = = = Public = = = //

        public void Setup(MercID _mercId)
        {
            _setupCalled = true;

            _MercID = _mercId;
        }

        // = = = Callbacks/Events = = = //

        void OnAttackImpact(GameObject target)
        {
            if (target && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(_MercID);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }

        // = = = ^

        bool IsValidTarget()
        {
            return currentTarget && currentTarget.CompareTag("Enemy");
        }


        GameObject GetTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            return targets.Length == 0 ? null : targets[Random.Range(0, targets.Length)];
        }
    }
}