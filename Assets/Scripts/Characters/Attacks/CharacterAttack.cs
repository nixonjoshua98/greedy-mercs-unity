using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    [RequireComponent(typeof(Character))]
    public abstract class CharacterAttack : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] Character character;

        [Header("Components")]
        [SerializeField] protected Animator anim;

        public Animator Anim { get { return anim; } }

        [Header("Properties")]
        [SerializeField, Range(0, 2.5f)] float delayBetweenAttacks = 0.25f;

        float attackTimer;

        float lastAttackTime;

        bool isAttacksToggled;

        protected bool CanAttack { get { return isAttacksToggled && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"); } }

        void Awake()
        {
            attackTimer = delayBetweenAttacks;
        }

        void Start()
        {
            lastAttackTime = Time.realtimeSinceStartup;

            attackTimer = delayBetweenAttacks;
        }

        void FixedUpdate()
        {
            if (CanAttack)
            {
                attackTimer -= Time.fixedDeltaTime;

                if (attackTimer <= 0.0f)
                {
                    attackTimer = delayBetweenAttacks;

                    if (GameManager.IsEnemyAvailable)
                    {
                        StartAttack();
                    }
                }
            }
        }

        public abstract void OnAttackEvent();

        void StartAttack()
        {
            anim.Play("Attack");
        }

        protected void DealDamage()
        {
            float timeSinceAttack = Time.realtimeSinceStartup - lastAttackTime;

            GameManager.TryDealDamageToEnemy(StatsCache.GetCharacterDamage(character.CharacterID) * (timeSinceAttack * Time.timeScale));

            lastAttackTime = Time.realtimeSinceStartup;
        }

        public void ToggleAttacking(bool val)
        {
            attackTimer = delayBetweenAttacks;

            isAttacksToggled = val;
        }
    }
}