using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    float stageSpawned;

    void Awake()
    {
        stageSpawned = GameManager.CurrentStage;
    }

    public void Process()
    {
        PlayerData.Gold += stageSpawned;
    }
}
