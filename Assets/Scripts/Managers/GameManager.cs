using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameDataStructs;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;

    [Space]

    [SerializeField] GameObject EnemyObject;

    EnemyData Enemy;
    StageData Stage;

    public bool IsEnemyAvailable {  get { return Enemy.IsEnemyAvailable; } }

    void Awake()
    {
        Instance = this;

        Enemy = new EnemyData();

        Stage = new StageData();
    }

    void Start()
    {
        CreateNewEnemy();

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    public bool TryDealDamageToEnemy(float amount)
    {
        if (IsEnemyAvailable)
        {
            DealDamageToEnemy(amount);

            return true;
        }

        return false;
    }

    void DealDamageToEnemy(float amount)
    {
        Enemy.Health.TakeDamage(amount);

        DamageNumbers.Instance.Add(amount);

        if (Enemy.Health.IsDead)
        {
            Enemy.Controller.StartDeathSequence();

            Invoke("OnEnemyDeath", 0.5f);

            Enemy.Controller = null;
        }

        else
        {
            Enemy.Controller.StartHurtSequence();
        }
    }

    void CreateNewEnemy()
    {
        GameObject spawnedEnemy = Instantiate(EnemyObject, SpawnPoint.position, Quaternion.identity);

        Enemy.Controller = spawnedEnemy.GetComponent<EnemyController>();
        
        Enemy.Health.Reset();
    }

    void OnEnemyDeath()
    {
        CreateNewEnemy();

        PlayerData.Gold += 1;

        Stage.AddKills(1);

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }
}
