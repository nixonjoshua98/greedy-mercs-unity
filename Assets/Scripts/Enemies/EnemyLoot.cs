namespace GM
{
    public class EnemyLoot : Core.GMMonoBehaviour, ILootDrop
    {
        protected CurrentStageState spawnStageState;

        void Awake()
        {
            spawnStageState = GameManager.Instance.State();
        }


        public virtual void Process()
        {
            BigDouble goldGainedForDefeat = App.Cache.GoldPerEnemyAtStage(spawnStageState.Stage);

            App.Data.Inv.Gold += goldGainedForDefeat;
        }
    }
}