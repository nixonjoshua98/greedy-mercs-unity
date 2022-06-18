namespace GM.Enemies
{
    public class BossLoot : EnemyLoot
    {
        public override void Process()
        {
            BigDouble gold = App.Values.GoldPerStageBossAtStage(spawnedStage);

            App.Inventory.Gold += gold;

            App.Inventory.GoldChanged.Invoke(gold);
        }
    }
}