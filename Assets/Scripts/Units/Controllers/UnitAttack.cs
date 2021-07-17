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

        public GameObjectEvent E_OnAttackImpact;

        bool isReady = true;
        bool onCooldown = false;
        bool isAttacking = false;

        GameObject currentTarget;

        protected float attackCooldown = 0.75f;


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


        public abstract Vector3 GetMoveVector(GameObject target);


        public abstract bool InAttackPosition(GameObject target);

        // = = = ^

        // = = = Callbacks/Events = = = //

        public virtual void OnAttackAnimationEvent()
        {
            isAttacking = false;

            OnAttackHit();
        }


        void OnAttackHit()
        {
            StartCooldown();

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
