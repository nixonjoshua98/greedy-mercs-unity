using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs
{    
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            GameState.Player.gold += StatsCache.StageEnemy.GetBossGold(stageSpawned);
        }
    }

}