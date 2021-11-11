using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GM.Targets;

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

        public Vector3 GetAttackPosition(AttackerTarget target)
        {
            if (target.Position.HasValue)
            {
                return (Vector3)target.Position;
            }

            Vector3 pos = target.Object.transform.position;

            pos = target.AttackID switch
            {
                0 => pos + Vector3.left,
                1 => pos + Vector3.right,
                _ => pos
            };

            return pos;
        }

        /// <summary>Animation Event</summary>
        public void OnAttackAnimation()
        {
            DealDamageToTarget();

            IsReady = true;
        }
    }
}
