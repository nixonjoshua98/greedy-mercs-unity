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
            BigDouble gold = App.Values.GoldPerEnemyAtStage(spawnedStage);

            App.Inventory.Gold += gold;

            App.Inventory.GoldChanged.Invoke(gold);
        }
    }
}