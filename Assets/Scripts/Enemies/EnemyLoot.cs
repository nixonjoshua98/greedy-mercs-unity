using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    int stageSpawned;

    void Awake()
    {
        stageSpawned = GameState.stage.stage;
    }

    public void Process()
    {
        GameState.player.gold += stageSpawned;
    }
}