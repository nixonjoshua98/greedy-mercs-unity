using GM.Units;
using System.Collections;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public abstract class AbstractAttackController : GM.Core.GMMonoBehaviour
    {
        /* Components */
        protected UnitAvatar Avatar;
        protected AbstractMercController Controller;

        /* Properties */
        readonly float CooldownTimer = 0.5f;

        protected UnitBase CurrentTarget;

        protected bool IsAttacking;
        protected bool IsOnCooldown;
        public bool HasControl { get; protected set; }
        public bool CanStartAttack { get => !IsOnCooldown && !IsAttacking; }

        protected virtual void Awake()
        {
            GetRequiredComponents();
        }

        protected virtual void GetRequiredComponents()
        {
            Controller = GetComponent<AbstractMercController>();
            Avatar = GetComponentInChildren<UnitAvatar>();

            GMLogger.WhenNull(Controller, "MercController is null");
            GMLogger.WhenNull(Avatar, "Avatar is null");
        }

        public void Stop()
        {
            HasControl = false;
            IsAttacking = false;
            CurrentTarget = null;
        }

        public virtual void StartAttack(GM.Units.UnitBase target)
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
