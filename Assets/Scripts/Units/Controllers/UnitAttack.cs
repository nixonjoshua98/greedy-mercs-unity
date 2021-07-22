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
        [Space]
        public UnitMovement movement;

        [Header("Animations")]
        public string attackAnimation = "Attacking";

        [Header("Events")]
        public GameObjectEvent E_OnAttackImpact;

        // Properties
        float attackCooldown = 1.0f;

        // Flags/States
        bool isReady = true;
        bool onCooldown = false;
        bool isAttacking = false;

        protected GameObject currentTarget;


        void Awake()
        {
            E_OnAttackImpact = new GameObjectEvent();
        }


        // = = = Public Methods = = = //
        public void TryAttack(GameObject target)
        {
            if (IsAvailable())
            {
                if (InAttackPosition(target))
                {
                    StartAttack(target);
                }
            }
        }


        public bool IsAttacking()
        {
            return isAttacking;
        }


        public abstract Vector3 GetTargetPosition(GameObject target);


        public abstract bool InAttackPosition(GameObject target);

        // = = = ^

        // = = = Callbacks/Events = = = //

        public void OnAttackAnimationEvent()
        {
            isAttacking = false;

            StartCooldown();

            OnAttackAnimation();
        }


        protected virtual void OnAttackAnimation()
        {
            E_OnAttackImpact.Invoke(currentTarget);
        }

        // = = = ^

        bool IsAvailable()
        {
            return isReady && !onCooldown;
        }


        void StartAttack(GameObject target)
        {
            isAttacking = true;

            currentTarget = target;

            movement.FaceTowards(target);

            anim.Play(attackAnimation);
        }


        void StartCooldown()
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