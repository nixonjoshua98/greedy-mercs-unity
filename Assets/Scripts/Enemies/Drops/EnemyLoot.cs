using UnityEngine;

namespace GM
{
    public class EnemyLoot : MonoBehaviour, ILootDrop
    {
        protected CurrentStageState spawnStageState;

        void Awake()
        {
            spawnStageState = GameManager.Instance.State();
        }


        public virtual void Process()
        {
            GameState.Player.gold += StatsCache.StageEnemy.GetEnemyGold(spawnStageState.Stage);
        }
    }
}