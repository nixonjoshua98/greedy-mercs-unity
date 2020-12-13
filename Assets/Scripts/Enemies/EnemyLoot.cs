using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    int stageSpawned;

    void Awake()
    {
        stageSpawned = GameManager.CurrentStage;
    }

    public void Process()
    {
        PlayerData.Gold += stageSpawned;
    }
}
