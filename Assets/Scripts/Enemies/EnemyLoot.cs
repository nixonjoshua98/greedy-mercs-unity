﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{    
    public class EnemyLoot : LootDrop
    {
        protected int stageSpawned;

        void Awake()
        {
            stageSpawned = GameState.Stage.currentStage;
        }

        public override void Process()
        {
            GameState.Player.gold += StatsCache.StageEnemy.GetEnemyGold(stageSpawned);
        }
    }
}