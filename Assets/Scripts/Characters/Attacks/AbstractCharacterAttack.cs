using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace GM.Characters
{
    using GM.Events;

    public abstract class AbstractCharacterAttack : ExtendedMonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected Animator anim;

        [Header("Animations")]
        [SerializeField] protected string AttackAnimation = "Attacking";

        [Header("Properties")]
        [SerializeField] float attackCooldown = 1.0f;

        bool isAttacking = false;
        bool isOnCooldown = false;

        // Private Flags
        float _spawnTime;

        protected GameObject AttackTarget { get; private set; }

        // Events
        public GameObjectEvent E_OnAttackHit;

        void Awake()
        {
            _spawnTime = Time.time;

            E_OnAttackHit = new GameObjectEvent();
        }

        public virtual void Attack(GameObject obj)
        {
            isAttacking = true;

            AttackTarget = obj;
        }

        // = = = Animation Event = = = //
        public virtual void OnAttackAnimationEvent()
        {
            OnAttackHit();
        }

        public virtual void OnAttackHit()
        {
            E_OnAttackHit.Invoke(AttackTarget);

            isAttacking = false;

            StartCooldown();
        }

        public virtual bool IsAvailable()
        {
            bool timeFlag = (Time.time - _spawnTime) >= 3.0f;

            return timeFlag && (!isAttacking && !isOnCooldown);
        }


        public virtual bool CanAttack(GameObject obj)
        {
            return IsAvailable();
        }


        // Trigger the cooldown, and wait out the duration
        void StartCooldown()
        {
            IEnumerator WaitForCooldown()
            {
                yield return new WaitForSecondsRealtime(attackCooldown);

                isOnCooldown = false;
            }

            isOnCooldown = true;

            StartCoroutine(WaitForCooldown());
        }
    }
}