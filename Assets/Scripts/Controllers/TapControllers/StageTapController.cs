using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class StageTapController : TapController
    {
        protected override void OnClick(Vector3 worldPos)
        {
            ActivateParticles(worldPos);

            GameManager.TryDealDamageToEnemy(StatsCache.GetTapDamage());

            Events.OnPlayerClick.Invoke();
        }
    }
}