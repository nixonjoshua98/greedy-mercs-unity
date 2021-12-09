using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Units
{
    public abstract class UnitAttack : MonoBehaviour
    {
        [Header("Components")]
        public Animator anim;

        // Properties
        float attackCooldown = 0.5f;

        public bool IsAttacking { get; private set; } = false;
        public bool OnCooldown { get; private set; } = false;
        public bool IsToggled { get; private set; } = true;

        public UnityEvent<GameObject> E_OnAttackImpact { get; private set; } = new UnityEvent<GameObject>();

        // Components
        UnitMovement movement;
        AnimationStrings animations;

        protected GameObject currentTarget;

        void Awake()
        {
            GetComponents();
        }


        void GetComponents()
        {
            animations  = GetComponentInChildren<AnimationStrings>();
            movement    = GetComponent<UnitMovement>();
        }

        public void Enable()
        {
            IsToggled = true;
            IsAttacking = false;
        }


        public void Disable()
        {
            IsAttacking = false;
            IsToggled = false;

            currentTarget = null;
        }


        public void Process(GameObject newTarget)
        {
            if (IsToggled)
            {
                if (!IsAttacking && !OnCooldown && InAttackPosition(newTarget))
                {
                    StartAttack(newTarget);
                }

                // Attack is currently not active and unavailable
                // eg. We are currently free to move etc.
                else if (!IsAttacking)
                {
                    if (!InAttackPosition(newTarget))
                    {
                        MoveTowardsTargetNewTarget(newTarget);
                    }

                    // Avoid the 'moving while idle' issue
                    else if (anim.GetCurrentAnimatorStateInfo(0).IsName(animations.Walk))
                    {
                        anim.Play(animations.Idle);
                    }
                }
            }
        }


        // Can be used to bypass checks such as positioning
        public void DirtyAttack(GameObject newTarget)
        {
            if (!OnCooldown && !IsAttacking)
            {
                StartAttack(newTarget);
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

            OnAttackAnimation();

            Cooldown();
        }


        protected virtual void OnAttackAnimation()
        {
            if (currentTarget)
                E_OnAttackImpact.Invoke(currentTarget);
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

                OnCooldown = false;
            }

            OnCooldown = true;

            StartCoroutine(WaitForCooldown());
        }
    }
}