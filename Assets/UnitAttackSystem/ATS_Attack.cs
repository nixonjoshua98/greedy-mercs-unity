using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    using GM.Events;
    public class ATS_Attack : MonoBehaviour
    {
        [Header("Components")]
        public Animator anim;

        [Header("Animations")]
        public string attackAnimString = "Attacking";

        public GameObjectEvent E_OnAttackImpact;

        bool isReady = true;
        bool onCooldown = false;
        bool isAttacking = false;

        GameObject currentTarget;

        float attackRange = 1.25f;
        float attackCooldown = 1.25f;

        private void Awake()
        {
            E_OnAttackImpact = new GameObjectEvent();
        }


        // = = = Public Methods = = = //
        public void TryAttack(GameObject target)
        {
            if (IsAvailable())
            {
                if (IsInAttackPosition(target))
                {
                    StartAttack(target);
                }
            }
        }

        public bool IsAvailable()
        {
            return isReady && !onCooldown;
        }


        public bool IsAttacking()
        {
            return isAttacking;
        }


        public Vector3 GetMoveVector(GameObject target)
        {
            return target.transform.position;
        }


        public bool IsInAttackPosition(GameObject target)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);

            return dist <= attackRange;
        }

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


        void StartAttack(GameObject target)
        {
            isAttacking = true;

            currentTarget = target;

            anim.Play(attackAnimString);
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
