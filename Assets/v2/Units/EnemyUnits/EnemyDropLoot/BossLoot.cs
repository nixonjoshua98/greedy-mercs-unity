﻿namespace GM.Enemies
{
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            BigDouble gold = App.GMCache.GoldPerStageBossAtStage(spawnedStage);

            App.Inventory.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}