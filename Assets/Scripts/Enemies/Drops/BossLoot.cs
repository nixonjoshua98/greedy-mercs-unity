using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{    
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            UserData.Get.Inventory.Gold += StatsCache.StageEnemy.GetBossGold(spawnStageState.Stage);
        }
    }

}