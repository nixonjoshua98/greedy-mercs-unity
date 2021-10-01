using UnityEngine;

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
            App.Data.Inv.Gold += StatsCache.StageEnemy.GetEnemyGold(spawnStageState.Stage);
        }
    }
}