using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public class ATS_MercController : ExtendedMonoBehaviour
    {
        [Header("Components")]
        public Animator anim;

        GameObject currentFocusTarget;

        ATS_Attack normalAttack;

        void Start()
        {
            GetComponents();
            SubscribeToEvents();
        }

        void GetComponents()
        {
            normalAttack = GetComponent<ATS_Attack>();
        }

        void SubscribeToEvents()
        {

        }

        void FixedUpdate()
        {
            if (currentFocusTarget)
            {
                if (!IsBusy())
                {
                    normalAttack.Process(currentFocusTarget);
                }
            }
        }

        protected override void PeriodicUpdate()
        {
            // Attempt to focus onto a new target
            if (currentFocusTarget == null)
            {
                currentFocusTarget = GetNewFocusTarget();

                if (currentFocusTarget == null)
                {
                    HandleNoAvailableTarget();
                }
            }
        }


        bool IsBusy()
        {
            return normalAttack.InUse();
        }

        // = = = Handles = = = //
        void HandleNoAvailableTarget()
        {
            anim.Play("Idle");
        }
        // = = = ^

        GameObject GetNewFocusTarget()
        {
            return null;
        }
    }
}