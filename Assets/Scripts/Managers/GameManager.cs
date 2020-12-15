using System.Collections;
using System.Collections.Generic;

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

        Debug.Log(Application.persistentDataPath);
    }

    void Start()
    {
        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(GameState.stage.stage, GameState.stage.enemy);
    }

    // This is the only method which should be dealing damage to the enemy
    public static void TryDealDamageToEnemy(float amount)
    {
        if (Instance.CurrentEnemy != null)
        {
            EnemyHealth health = Instance.CurrentEnemy.GetComponent<EnemyHealth>();

            Instance.damageNumbers.Add(amount);

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
        GameState.stage.AddKill();

        SpawnNextEnemy();
    }

    void OnBossDeath()
    {
        GameState.stage.AdvanceStage();

        SpawnNextEnemy();

        EventManager.OnStageUpdate.Invoke(GameState.stage.stage, GameState.stage.enemy);
    }

    void SpawnNextEnemy()
    {
        StartCoroutine(ISpawnNextEnemy());
    }

    IEnumerator ISpawnNextEnemy()
    {
        yield return new WaitForSeconds(0.2f);

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