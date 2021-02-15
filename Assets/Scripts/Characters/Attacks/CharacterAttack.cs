using System;

using UnityEngine;

namespace GreedyMercs
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Animator anim;

        public Animator Anim { get { return anim; } }

        [Header("Properties")]
        [SerializeField, Range(0, 2.5f)] float delayBetweenAttacks = 0.25f;

        bool CanAttack { get { return isAttacksToggled && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"); } }
        public bool IsAttackReady { get { return CanAttack && attackTimer <= 0.0f; } }

        bool isAttacksToggled;

        float lastAttackTime;
        float attackTimer;

        Action<float> AttackCallback;

        void Awake()
        {
            ToggleAttacking(true);
        }

        void FixedUpdate()
        {
            if (CanAttack)
            {
                attackTimer = Mathf.Max(0.0f, attackTimer - Time.fixedDeltaTime);
            }
        }

        public void SetAttackCallback(Action<float> callback)
        {
            AttackCallback = callback;
        }

        // === Attack Methods ===

        public void StartAttack()
        {
            anim.Play("Attack");
        }

        public abstract void OnAttackAnimationFinished();

        protected void OnAttackHit()
        {
            AttackCallback(Time.timeSinceLevelLoad - lastAttackTime);

            ResetAttackTimer();
        }

        public void ToggleAttacking(bool val)
        {
            ResetAttackTimer();

            isAttacksToggled = val;
        }

        protected void ResetAttackTimer()
        {
            attackTimer     = delayBetweenAttacks;
            lastAttackTime  = Time.timeSinceLevelLoad;
        }
    }
}