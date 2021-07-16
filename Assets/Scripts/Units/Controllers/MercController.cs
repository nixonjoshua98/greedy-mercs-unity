using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class MercController : UnitController
    {
        [SerializeField] MercID ID;

        [Header("Components")]
        public Animator anim;

        [Space]

        public UnitMovement movement;
        public UnitAttack attack;

        GameObject currentFocusTarget;

        void Start()
        {
            SubscribeToEvents();
        }


        void SubscribeToEvents()
        {
            attack.E_OnAttackImpact.AddListener(OnAttackImpact);
        }


        void FixedUpdate()
        {
            if (currentFocusTarget)
            {
                // We have a target and an attack is available so we
                // process it (eg. move towards a valid attack position)
                attack.TryAttack(currentFocusTarget);


                // Attack is currently not active and unavailable
                // eg. We are currently free to move etc.
                if (!attack.IsAttacking())
                {
                    if (!attack.InAttackPosition(currentFocusTarget))
                    {
                        Vector3 moveVector = attack.GetMoveVector(currentFocusTarget);

                        movement.MoveTowards(moveVector);
                    }

                    // Avoid the 'moving while idle' issue
                    else if (movement.IsCurrentAnimationWalk())
                    {
                        anim.Play("Idle");
                    }
                }
            }
        }


        protected override void PeriodicUpdate()
        {
            // Attempt to focus onto a new target
            if (!IsValidTarget())
            {
                currentFocusTarget = GetNewFocusTarget();
            }
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

        bool IsValidTarget()
        {
            return currentFocusTarget && currentFocusTarget.CompareTag("Enemy");
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