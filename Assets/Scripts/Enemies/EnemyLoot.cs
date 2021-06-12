using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{    
    public class EnemyLoot : LootDrop
    {
        protected int stageSpawned;

        void Awake()
        {
            CurrentStageState state = GameManager.Instance.GetState();

            stageSpawned = state.currentStage;
        }

        public override void Process()
        {
            GameState.Player.gold += StatsCache.StageEnemy.GetEnemyGold(stageSpawned);
        }
    }
}