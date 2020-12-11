using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;

    [SerializeField] GameObject EnemyObject;
    [SerializeField] GameObject BossObject;

    StageData Stage;

    GameObject CurrentEnemy;

    public int CurrentStage { get { return Stage.CurrentStage; } }

    void Awake()
    {
        Instance = this;

        Stage = new StageData();
    }

    void Start()
    {
        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    public void TryDealDamageToEnemy(float amount)
    {
        if (CurrentEnemy != null)
        {
            EnemyHealth health = CurrentEnemy.GetComponent<EnemyHealth>();

            if (!health.IsDead)
            {
                health.TakeDamage(amount);

                if (health.IsDead)
                {
                    switch (CurrentEnemy.tag)
                    {
                        case "Enemy":
                            OnEnemyDeath();
                            break;

                        case "BossEnemy":
                            OnBossDeath();
                            break;
                    }

                }
            }
        }
    }

    void OnEnemyDeath()
    {
        Stage.AddKill();

        SpawnNextEnemy();
    }

    void OnBossDeath()
    {
        Stage.AdvanceStage();

        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    void SpawnNextEnemy()
    {
        if (Stage.IsStageCompleted)
            StartCoroutine(SpawnBossEnemy());

        else
            StartCoroutine(SpawnEnemy());
    }

    void CreateNewEnemy(GameObject ObjectToSpawn)
    {
        CurrentEnemy = Instantiate(ObjectToSpawn, SpawnPoint.position, Quaternion.identity);
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
}
