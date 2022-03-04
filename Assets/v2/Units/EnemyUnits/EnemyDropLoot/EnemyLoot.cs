namespace GM.Enemies
{
    public interface ILootDrop
    {
        public void Process();
    }

    public class EnemyLoot : Core.GMMonoBehaviour, ILootDrop
    {
        protected int spawnedStage;

        void Awake()
        {
            spawnedStage = App.GMData.GameState.Stage;
        }


        public virtual void Process()
        {
            BigDouble gold = App.GMCache.GoldPerEnemyAtStage(spawnedStage);

            App.GMData.Inv.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}