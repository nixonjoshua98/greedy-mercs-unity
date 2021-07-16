using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    using GM.Events;

    public class StageTapController : TapController
    {
        protected override void OnClick(Vector3 worldPos)
        {
            ActivateParticles(worldPos);

            GameObject target = GetNewFocusTarget();

            if (target && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = StatsCache.GetTapDamage();

                hp.TakeDamage(dmg);
            }

            GlobalEvents.OnPlayerClick.Invoke();
        }

        GameObject GetNewFocusTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            if (targets.Length == 0)
                return null;

            return targets[Random.Range(0, targets.Length)];
        }
    }
}