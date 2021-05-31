using System.Collections;

using UnityEngine;

namespace GM.Characters
{
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

        protected GameObject AttackTarget { get; private set; }

        // Components
        AbstractCharacterController characterController;

        void Awake()
        {
            GetComponents();
        }

        void GetComponents()
        {
            characterController = GetComponent<AbstractCharacterController>();
        }

        public virtual void Attack(GameObject obj)
        {
            isAttacking = true;

            AttackTarget = obj;
        }

        // = = = Animation Event = = = //
        public virtual void OnAttackAnimationEvent()
        {
            characterController.OnAttack();

            isAttacking = false;

            StartCooldown();
        }

        public virtual bool IsAvailable()
        {
            return !isAttacking && !isOnCooldown;
        }

        void StartCooldown()
        {
            isOnCooldown = true;

            StartCoroutine(WaitForCooldown());
        }

        IEnumerator WaitForCooldown()
        {
            yield return new WaitForSecondsRealtime(attackCooldown);

            isOnCooldown = false;
        }
    }
}