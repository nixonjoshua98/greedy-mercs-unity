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
            UserData.Get.Inventory.Gold += StatsCache.StageEnemy.GetEnemyGold(spawnStageState.Stage);
        }
    }
}