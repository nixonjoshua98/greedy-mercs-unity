using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    using GM.Events;

    using GM.Targetting;
    public class StageTapController : TapController
    {
        [SerializeField] FriendlyTargetter enemyTargetter;

        protected override void OnClick(Vector3 worldPos)
        {
            ActivateParticles(worldPos);

            GameObject target = enemyTargetter.GetTarget();

            if (target && target.TryGetComponent(out HealthController hp))
            {
                BigDouble dmg = StatsCache.GetTapDamage();

                hp.TakeDamage(dmg);
            }

            GlobalEvents.OnPlayerClick.Invoke();
        }
    }
}