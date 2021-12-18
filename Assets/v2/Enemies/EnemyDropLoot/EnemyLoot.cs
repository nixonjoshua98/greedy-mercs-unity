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
            spawnedStage = GameManager.Instance.State.Stage;
        }


        public virtual void Process()
        {
            BigDouble gold = App.Cache.GoldPerEnemyAtStage(spawnedStage);

            App.Data.Inv.Gold += gold;

            App.Events.GoldChanged.Invoke(gold);
        }
    }
}