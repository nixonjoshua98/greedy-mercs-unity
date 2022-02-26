using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Enemies
{    
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            BigDouble gold = App.Cache.GoldPerStageBossAtStage(spawnedStage);

            App.Data.Inv.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}