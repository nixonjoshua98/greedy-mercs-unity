using SRC.Units;
using System.Collections;
using UnityEngine;

namespace SRC.Mercs.Controllers
{
    public abstract class AbstractAttackController : SRC.Core.GMMonoBehaviour
    {
        /* Components */
        protected UnitAvatar Avatar;
        protected AbstractMercController Controller;

        /* Properties */
        private readonly float CooldownTimer = 0.5f;

        protected GameObject CurrentTarget;

        protected bool IsAttacking;
        protected bool IsOnCooldown;

        public bool CanStartAttack { get => !IsOnCooldown && !IsAttacking; }

        protected virtual void Awake()
        {
            GetRequiredComponents();
        }

        protected virtual void GetRequiredComponents()
        {
            Controller = GetComponent<AbstractMercController>();
            Avatar = GetComponentInChildren<UnitAvatar>();
        }

        public virtual void StartAttack(GameObject target)
        {
            IsAttacking = true;
            CurrentTarget = target;
        }

        protected void StartCooldown()
        {
            StartCoroutine(CooldownTask(CooldownTimer));
        }

        private IEnumerator CooldownTask(float seconds)
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(seconds);
            IsOnCooldown = false;
        }
    }
}
