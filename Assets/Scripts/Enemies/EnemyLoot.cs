using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : LootDrop
{
    protected int stageSpawned;

    void Awake()
    {
        stageSpawned = GameState.Stage.stage;
    }

    public override void Process()
    {
        GameState.Player.gold += StatsCache.GetEnemyGold(stageSpawned);
    }
}