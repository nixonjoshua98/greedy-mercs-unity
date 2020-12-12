using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;
    [Space]
    [SerializeField] GameObject[] EnemyObjects;

    StageData Stage;

    GameObject CurrentEnemy;

    public static int CurrentStage { get { return Instance.Stage.CurrentStage; } }

    void Awake()
    {
        Instance = this;

        Stage = new StageData();

        EventManager.OnBossSpawned.AddListener(OnBossSpawned);
        EventManager.OnFailedToKillBoss.AddListener(OnFailedToKillBoss);
    }

    void Start()
    {
        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }

    // This is the only method which should be dealing damage to the enemy
    public static void TryDealDamageToEnemy(float amount)
    {
        if (Instance.CurrentEnemy != null)
        {
            EnemyHealth health = Instance.CurrentEnemy.GetComponent<EnemyHealth>();

            DamageNumbers.Instance.Add(amount);

            if (!health.IsDead)
            {
                health.TakeDamage(amount);

                if (health.IsDead)
                {
                    switch (Instance.CurrentEnemy.tag)
                    {
                        case "Enemy":
                            Instance.OnEnemyDeath();
                            break;

                        case "BossEnemy":
                            Instance.OnBossDeath();
                            break;
                    }
                }
            }
        }
    }

    // Called from BossBattleManager
    public static void TrySkipToBoss()
    {
        if (!BossBattleManager.IsAvoidingBoss && Instance.Stage.IsStageCompleted)
        {
            if (Instance.CurrentEnemy != null)
                Destroy(Instance.CurrentEnemy);

            BossBattleManager.StartBossBattle();
        }
    }

    // UnityEvent Listener
    public void OnFailedToKillBoss()
    {
        SpawnNextEnemy();
    }

    // UnityEvent Listener
    void OnBossSpawned(GameObject boss)
    {
        CurrentEnemy = boss;
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

        EventManager.OnStageUpdate.Invoke(CurrentStage, Stage.CurrentEnemy);
    }

    void SpawnNextEnemy()
    {
        if (!BossBattleManager.IsAvoidingBoss && Stage.IsStageCompleted)
        {
            BossBattleManager.StartBossBattle();
        }
        else
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        CurrentEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity);

        EventManager.OnStageUpdate.Invoke(Stage.CurrentStage, Stage.CurrentEnemy);
    }
}