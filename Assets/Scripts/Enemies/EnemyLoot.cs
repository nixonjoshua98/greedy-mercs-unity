using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : LootDrop
{
    protected int stageSpawned;

    void Awake()
    {
        stageSpawned = GameState.stage.stage;
    }

    public override void Process()
    {
        GameState.player.gold += StatsCache.GetEnemyGold(stageSpawned);
    }
}