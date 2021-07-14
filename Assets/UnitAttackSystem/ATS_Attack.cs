using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Units
{
    using GM.Events;
    public abstract class ATS_Attack : MonoBehaviour
    {
        [Header("Components")]
        public Animator anim;
        [Space]
        public ATS_Movement movement;

        [Header("Animations")]
        public string attackAnimString = "Attacking";

        public GameObjectEvent E_OnAttackImpact;

        bool isReady = true;
        bool onCooldown = false;
        bool isAttacking = false;

        GameObject currentTarget;

        protected float attackCooldown = 1.25f;


        private void Awake()
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

        void MoveToIdle()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {

            }
        }

        bool IsAvailable()
        {
            return isReady && !onCooldown;
        }


        void StartAttack(GameObject target)
        {
            isAttacking = true;

            currentTarget = target;

            movement.FaceTowards(target);

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
