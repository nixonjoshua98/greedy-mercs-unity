using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class MercController : UnitController
    {
        [SerializeField] MercID mercId;
        public MercID MercId { get { return mercId; } }

        [Header("Components")]
        public Animator anim;

        [Header("Animations")]
        [SerializeField] string idleAnimation = "Idle";

        [Space]

        public UnitMovement movement;
        public UnitAttack attack;

        GameObject currentTarget;

        bool _setupCalled = false;

        void Start()
        {
            SubscribeToEvents();

            if (!_setupCalled)
                Debug.LogError($"Setup not called on {name}");
        }


        void SubscribeToEvents()
        {
            attack.E_OnAttackImpact.AddListener(OnAttackImpact);
        }


        void FixedUpdate()
        {
            if (IsValidTarget())
            {
                // We have a target and an attack is available so we
                // process it (eg. move towards a valid attack position)
                attack.TryAttack(currentTarget);


                // Attack is currently not active and unavailable
                // eg. We are currently free to move etc.
                if (!attack.IsAttacking())
                {
                    if (!attack.InAttackPosition(currentTarget))
                    {
                        movement.MoveTowards(attack.GetMoveVector(currentTarget));
                    }

                    // Avoid the 'moving while idle' issue
                    else if (movement.IsCurrentAnimationWalk())
                    {
                        anim.Play(idleAnimation);
                    }
                }
            }
            else if (!attack.IsAttacking())
            {
                currentTarget = GetNewFocusTarget();

                movement.MoveDirection(Vector2.right);
            }
        }

        // = = = Public = = = //

        public void Setup(MercID _mercId)
        {
            _setupCalled = true;

            mercId = _mercId;
        }

        // = = = Callbacks/Events = = = //

        void OnAttackImpact(GameObject target)
        {
            if (target && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(mercId);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }

        // = = = ^

        bool IsValidTarget()
        {
            return currentTarget && currentTarget.CompareTag("Enemy");
        }


        GameObject GetNewFocusTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            if (targets.Length == 0)
                return null;

            return targets[Random.Range(0, targets.Length)];
        }
    }
}