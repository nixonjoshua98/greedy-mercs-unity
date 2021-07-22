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

        [Header("Animations")]
        [SerializeField] string idleAnimation = "Idle";

        [Space]

        public UnitMovement movement;
        public UnitAttack attack;

        GameObject currentTarget;

        bool _setupCalled = false;

        void Start()
        {
            GetComponents();

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

        void GetComponents()
        {

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
                        movement.MoveTowards(attack.GetTargetPosition(currentTarget));
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