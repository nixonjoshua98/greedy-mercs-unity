using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;
    [Space]
    [SerializeField] GameObject[] EnemyObjects;
    [Space]
    [SerializeField] DamageNumbers damageNumbers;

    GameObject CurrentEnemy;

    public static bool IsEnemyAvailable {  get { return Instance.CurrentEnemy != null; } }

    void Awake()
    {
        Instance = this;

        EventManager.OnBossSpawned.AddListener(OnBossSpawned);
        EventManager.OnFailedToKillBoss.AddListener(OnFailedToKillBoss);

        Debug.Log(Application.persistentDataPath);
    }

    void Start()
    {
        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(GameState.stage.stage, GameState.stage.enemy);
    }

    // This is the only method which should be dealing damage to the enemy
    public static void TryDealDamageToEnemy(double amount)
    {
        if (Instance.CurrentEnemy != null)
        {
            if (Instance.CurrentEnemy.TryGetComponent(out Health health))
            {
                if (!health.IsDead)
                {
                    Instance.damageNumbers.Add(amount);

                    health.TakeDamage(amount);

                    if (health.IsDead)
                    {
                        Instance.OnEnemyDeath();

                        Destroy(Instance.CurrentEnemy);
                    }
                }
            }
        }
    }

    // Called from BossBattleManager
    public static void TrySkipToBoss()
    {
        if (!BossBattleManager.IsAvoidingBoss && GameState.stage.isStageCompleted)
        {
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
        if (Instance.CurrentEnemy != null)
        {
            Destroy(Instance.CurrentEnemy);
        }

        CurrentEnemy = boss;
    }

    void OnEnemyDeath()
    {
        if (Instance.CurrentEnemy.TryGetComponent(out LootDrop loot))
            loot.Process();
        else
        {
            Debug.LogWarning("Enemy did not have a LootDrop component");
        }

        if (CurrentEnemy.CompareTag("Enemy"))
        {
            GameState.stage.AddKill();
        }

        else if (CurrentEnemy.CompareTag("BossEnemy"))
        {
            GameState.stage.AdvanceStage();
        }

        EventManager.OnStageUpdate.Invoke(GameState.stage.stage, GameState.stage.enemy);

        SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        StartCoroutine(ISpawnNextEnemy());
    }

    IEnumerator ISpawnNextEnemy()
    {
        yield return new WaitForSeconds(0.25f);

        if (!BossBattleManager.IsAvoidingBoss && GameState.stage.isStageCompleted)
        {
            BossBattleManager.StartBossBattle();
        }
        else
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        CurrentEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity);

        EventManager.OnStageUpdate.Invoke(GameState.stage.stage, GameState.stage.enemy);
    }
}