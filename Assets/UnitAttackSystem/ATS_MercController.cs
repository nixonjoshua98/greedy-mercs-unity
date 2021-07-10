using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    public class ATS_MercController : ExtendedMonoBehaviour
    {
        [SerializeField] MercID ID;

        [Header("Components")]
        public Animator anim;

        [Space]

        public ATS_Movement movement;
        public ATS_Attack attack;

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
                if (attack.IsAvailable())
                {
                    attack.TryAttack(currentFocusTarget);
                }


                // Attack is currently not active and unavailable
                // eg. We are currently free to move etc.
                if (!attack.IsAttacking())
                {
                    if (!attack.IsInAttackPosition(currentFocusTarget))
                    {
                        Vector3 moveVector = attack.GetMoveVector(currentFocusTarget);

                        movement.MoveTowards(moveVector);
                    }
                }
            }
        }


        protected override void PeriodicUpdate()
        {
            // Attempt to focus onto a new target
            if (currentFocusTarget == null)
            {
                currentFocusTarget = GetNewFocusTarget();
            }
        }

        // = = = Callbacks/Events = = = //

        void OnAttackImpact(GameObject target)
        {
            // Target is already dead
            if (!target)
                return;


            if (target.TryGetComponent(out AbstractHealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(ID);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }

        // = = = ^


        GameObject GetNewFocusTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            if (targets.Length == 0)
                return null;

            return targets[0];
        }
    }
}