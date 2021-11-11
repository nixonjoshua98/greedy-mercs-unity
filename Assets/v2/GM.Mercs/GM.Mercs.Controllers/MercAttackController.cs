using GM.Targets;
using UnityEngine;

namespace GM.Mercs.Controllers
{
    public class MercAttackController : MercComponent
    {
        [Header("References")]
        [SerializeField] AnimationStrings animStrings;
        [SerializeField] Animator anim;

        GameObject currentAttackTarget;

        public bool IsReady { get; private set; } = true;

        public void AttackTarget(GameObject target)
        {
            IsReady = false;

            currentAttackTarget = target;

            anim.Play(animStrings.Attack);
        }

        void DealDamageToTarget()
        {
            if (currentAttackTarget == null)
            {
                Debug.Log("Attempted to attack destroyed object");
            }

            else if (!currentAttackTarget.TryGetComponent(out HealthController health))
            {
                Debug.Log("Target is missing 'HealthController' component");
            }
            else if (health.CurrentHealth == 0)
            {
                Debug.Log("Attempted to attack an already dead unit");
            }
            else
            {
                BigDouble dmg = StatsCache.TotalMercDamage(ID);

                StatsCache.ApplyCritHit(ref dmg);

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
