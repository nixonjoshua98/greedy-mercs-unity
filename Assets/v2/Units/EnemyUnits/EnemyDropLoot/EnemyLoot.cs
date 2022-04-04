namespace GM.Enemies
{
    public interface ILootDrop
    {
        public void Process();
    }

    public class EnemyLoot : Core.GMMonoBehaviour, ILootDrop
    {
        protected int spawnedStage;

        private void Awake()
        {
            spawnedStage = App.GameState.Stage;
        }


        public virtual void Process()
        {
            BigDouble gold = App.GMCache.GoldPerEnemyAtStage(spawnedStage);

            App.Inventory.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}