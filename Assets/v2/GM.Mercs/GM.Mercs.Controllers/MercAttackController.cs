using GM.Targets;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MercAttackController : MercComponent
    {
        GameObject currentAttackTarget;

        public bool IsReady { get; private set; } = true;

        public void AttackTarget(GameObject target)
        {
            IsReady = false;

            currentAttackTarget = target;

            AvatarAnimator.Play(Animations.Attack);
        }

        void DealDamageToTarget()
        {
            if (currentAttackTarget == null)
            {
            }
            else if (!currentAttackTarget.TryGetComponent(out HealthController health))
            {
            }
            else if (health.CurrentHealth == 0)
            {
            }
            else
            {
                BigDouble dmg = App.Cache.MercDamage(App.Data.Mercs.GetMerc(ID));

                App.Cache.ApplyCritHit(ref dmg);

                health.TakeDamage(dmg);
            }
        }

        public Vector2 GetAttackPosition(Target target)
        {
            if (target.AttackPosition.HasValue)
            {
                return (Vector3)target.AttackPosition;
            }

            return target.Object.transform.position;
        }

        /// <summary>Animation Event</summary>
        public void OnAttackAnimation()
        {
            DealDamageToTarget();

            IsReady = true;
        }
    }
}
