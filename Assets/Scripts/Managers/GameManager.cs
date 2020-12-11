using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;

    [Space]

    [SerializeField] GameObject EnemyObject;
    [SerializeField] GameObject BossObject;

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
        CreateNewEnemy(EnemyObject);

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
                if (Enemy.IsBoss)
                    OnBossDeath();

                else
                    OnEnemyDeath();
            }
            else
                OnEnemyHurt();
        }
    }

    void OnEnemyHurt()
    {
        Enemy.Controller.OnHurt();
    }

    void OnEnemyDeath()
    {
        Enemy.Controller.OnDeath();

        Stage.AddKill();

        if (Stage.IsStageCompleted)
        {
            StartCoroutine(SpawnBossEnemy());
        }
        else
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    void OnBossDeath()
    {
        Enemy.Controller.OnDeath();

        Stage.AdvanceStage();

        StartCoroutine(SpawnEnemy());

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    IEnumerator SpawnBossEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        CreateNewEnemy(BossObject);

        EventManager.OnBossSpawned.Invoke();
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        CreateNewEnemy(EnemyObject);

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    void CreateNewEnemy(GameObject ObjectToSpawn)
    {
        GameObject spawnedEnemy = Instantiate(ObjectToSpawn, SpawnPoint.position, Quaternion.identity);

        Enemy.Controller = spawnedEnemy.GetComponent<EnemyController>();

        Enemy.Health = spawnedEnemy.GetComponent<EnemyHealth>();
    }
}
