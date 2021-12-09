﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{    
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            App.Data.Inv.Gold += App.Cache.GoldPerStageBossAtStage(spawnStageState.Stage);
        }
    }
}