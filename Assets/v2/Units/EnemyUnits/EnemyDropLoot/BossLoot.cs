using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Enemies
{    
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            BigDouble gold = App.GMCache.GoldPerStageBossAtStage(spawnedStage);

            App.DataContainers.Inv.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}