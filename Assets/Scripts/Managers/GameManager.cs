using System.Collections;

using UnityEngine;

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
    }

    void Start()
    {
        SpawnNextEnemy();
    }

    // This is the only method which should be dealing damage to the enemy
    public static void TryDealDamageToEnemy(BigDouble amount)
    {
        if (Instance.CurrentEnemy != null)
        {
            if (Instance.CurrentEnemy.TryGetComponent(out Health health))
            {
                if (!health.IsDead)
                {
                    Color col = StatsCache.ApplyCritHit(ref amount) ? Color.red : Color.white;

                    Instance.damageNumbers.Add(amount, col);

                    health.TakeDamage(amount);

                    EventManager.OnEnemyHurt.Invoke(health);

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
        if (!BossBattleManager.IsAvoidingBoss && GameState.Stage.isStageCompleted)
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

        // ===

        if (CurrentEnemy.CompareTag("Enemy"))
        {
            GameState.Stage.AddKill();

            EventManager.OnKillEnemy.Invoke();
        }

        else if (CurrentEnemy.CompareTag("BossEnemy"))
        {
            GameState.Stage.AdvanceStage();

            EventManager.OnNewStageStarted.Invoke();
        }

        SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        StartCoroutine(ISpawnNextEnemy());
    }

    IEnumerator ISpawnNextEnemy()
    {
        yield return new WaitForSeconds(0.25f);

        if (!BossBattleManager.IsAvoidingBoss && GameState.Stage.isStageCompleted)
        {
            BossBattleManager.StartBossBattle();
        }
        else
        {
            EventManager.OnStageUpdate.Invoke();

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        CurrentEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity);

        EventManager.OnEnemySpawned.Invoke();
    }
}