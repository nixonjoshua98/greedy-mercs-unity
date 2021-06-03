using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class StageTapController : TapController
    {
        protected override void OnClick(Vector3 worldPos)
        {
            ActivateParticles(worldPos);

            GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());

            GlobalEvents.OnPlayerClick.Invoke();
        }
    }
}