using System.Collections;

using UnityEngine;

namespace GM.Units
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


        // Public accessors
        public bool IsAttacking { get; private set; } = false;
        public bool HasAttackTarget { get { return AttackTarget != null; } }
        public bool IsAvailable
        { 
            get 
            {
                bool timeFlag = (Time.time - _spawnTime) >= 1.0f;

                return timeFlag && (!IsAttacking && !IsOnCooldown);
            } 
        }

        public bool IsOnCooldown { get; private set; } = false;
        

        // Private Flags
        float _spawnTime;


        // Currently target
        protected GameObject AttackTarget { get; private set; }


        // Events
        public GameObjectEvent E_OnAttackHit;


        void Awake()
        {
            _spawnTime = Time.time;

            E_OnAttackHit = new GameObjectEvent();
        }

        public void Process()
        {
            if (HasAttackTarget)
            {
                // Attack is available and we are within range
                if (AvailableToStartAttack())
                {
                    PerformAttack();
                }

                // We are currently not attacking, and we are outside attack range
                // so we need to move towards the target
                else if (!IsAttacking && !WithinValidAttackRange())
                {
                    MoveTowardsValidAttackPosition();
                }
            }
        }

        protected virtual void PerformAttack()
        {
            Attack(AttackTarget);

            anim.Play(AttackAnimation);
        }


        protected abstract void MoveTowardsValidAttackPosition();

        protected abstract bool WithinValidAttackRange();

        public virtual void Attack(GameObject obj)
        {
            IsAttacking = true;

            SetAttackTarget(obj);
        }


        // = = = Public Methods = = = //
        public void SetAttackTarget(GameObject target)
        {
            AttackTarget = target;
        }

        public bool AvailableToStartAttack()
        {
            return IsAvailable && WithinValidAttackRange();
        }

        // = = = Animation Event = = = //
        public virtual void OnAttackAnimationEvent()
        {
            OnAttackHit();
        }
        // = = = ^

        public virtual void OnAttackHit()
        {
            E_OnAttackHit.Invoke(AttackTarget);

            IsAttacking = false;

            StartCooldown();
        }


        public virtual bool CanAttack(GameObject obj)
        {
            return IsAvailable;
        }


        // Trigger the cooldown, and wait out the duration
        void StartCooldown()
        {
            IEnumerator WaitForCooldown()
            {
                yield return new WaitForSecondsRealtime(attackCooldown);

                IsOnCooldown = false;
            }

            IsOnCooldown = true;

            StartCoroutine(WaitForCooldown());
        }
    }
}