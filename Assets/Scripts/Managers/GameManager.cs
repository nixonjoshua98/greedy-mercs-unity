using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;

    [Space]

    [SerializeField] GameObject EnemyObject;

    EnemyData Enemy;
    StageData Stage;

    public bool IsEnemyAvailable {  get { return Enemy.IsAvailable; } }

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

    public void TryDealDamageToEnemy(float amount)
    {
        if (Enemy.IsAvailable)
        {
            Enemy.Health.TakeDamage(amount);

            DamageNumbers.Instance.Add(amount);

            if (Enemy.Health.IsDead)
            {
                OnEnemyDeath();
            }
            else
                OnEnemyHurt();
        }
    }

    void CreateNewEnemy()
    {
        GameObject spawnedEnemy = Instantiate(EnemyObject, SpawnPoint.position, Quaternion.identity);

        Enemy.Controller = spawnedEnemy.GetComponent<EnemyController>();
        
        Enemy.Health.Reset();
    }

    void OnEnemyHurt()
    {
        Enemy.Controller.StartHurtSequence();
    }

    void OnEnemyDeath()
    {
        Enemy.Controller.StartDeathSequence();

        PlayerData.Gold += 1;

        Stage.AddKill();
        
        Invoke("CreateNewEnemy", 0.5f);

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }
}
