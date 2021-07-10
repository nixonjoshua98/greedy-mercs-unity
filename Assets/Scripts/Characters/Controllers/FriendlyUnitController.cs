using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Units
{
    public class FriendlyUnitController : AbstractUnitController
    {
        [SerializeField] MercID assignedUnitId;

        protected override void OnAttackImpact(GameObject target)
        {
            // 'target' may be Null under some cases such as an attack being delayed
            if (target && target.TryGetComponent(out IHealthController hp))
            {
                BigDouble dmg = StatsCache.TotalMercDamage(assignedUnitId);

                StatsCache.ApplyCritHit(ref dmg);

                hp.TakeDamage(dmg);
            }
        }
    }
}
