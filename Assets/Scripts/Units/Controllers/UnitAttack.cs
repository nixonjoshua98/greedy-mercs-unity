using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    using GM.Events;

    public abstract class UnitAttack : MonoBehaviour
    {
        [Header("Components")]
        public Animator anim;

        [Header("Events")]
        public GameObjectEvent E_OnAttackImpact;

        // Properties
        float attackCooldown = 1.0f;

        // Flags/States
        bool onCooldown = false;

        public bool IsAttacking { get; private set; } = false;
        public bool IsAvailable { get { return !onCooldown; } }

        // Components
        UnitMovement movement;
        AnimationStrings animations;

        protected GameObject currentTarget;


        void Awake()
        {
            E_OnAttackImpact = new GameObjectEvent();

            GetComponents();
        }


        void GetComponents()
        {
            animations  = GetComponent<AnimationStrings>();
            movement    = GetComponent<UnitMovement>();
        }


        public void Stop()
        {
            IsAttacking = false;
            currentTarget = null;

            anim.Play(animations.Idle);
        }


        public void Process(GameObject newTarget)
        {
            // We have a target and an attack is available so we
            // process it (eg. move towards a valid attack position)
            if (IsAvailable && InAttackPosition(newTarget))
            {
                StartAttack(newTarget);
            }

            // Attack is currently not active and unavailable
            // eg. We are currently free to move etc.
            else if (!IsAttacking)
            {
                movement.FaceTowards(newTarget);

                if (!InAttackPosition(newTarget))
                {
                    MoveTowardsTargetNewTarget(newTarget);
                }

                // Avoid the 'moving while idle' issue
                else if (anim.IsName(animations.Walk))
                {
                    anim.Play(animations.Idle);
                }
            }
        }


        void MoveTowardsTargetNewTarget(GameObject target)
        {
            movement.MoveTowards(GetTargetPosition(target));
        }


        protected abstract Vector3 GetTargetPosition(GameObject target);
        protected abstract bool InAttackPosition(GameObject target);

        // = = = ^

        // = = = Callbacks/Events = = = //

        public void OnAttackAnimationEvent()
        {
            IsAttacking = false;

            Cooldown();

            OnAttackAnimation();
        }


        protected virtual void OnAttackAnimation()
        {
            if (currentTarget != null)
            {
                E_OnAttackImpact.Invoke(currentTarget);
            }
        }

        // = = = ^

        void StartAttack(GameObject target)
        {
            IsAttacking = true;

            currentTarget = target;

            movement.FaceTowards(target);

            anim.Play(animations.Attack);
        }


        void Cooldown()
        {
            IEnumerator WaitForCooldown()
            {
                yield return new WaitForSecondsRealtime(attackCooldown);

                onCooldown = false;
            }

            onCooldown = true;

            StartCoroutine(WaitForCooldown());
        }
    }
}