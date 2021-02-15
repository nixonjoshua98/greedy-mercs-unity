using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class GameManager : MonoBehaviour
    {
        static GameManager Instance = null;

        [SerializeField] Transform SpawnPoint;
        [Space]
        [SerializeField] GameObject[] EnemyObjects;
        [Space]
        [SerializeField] DamageNumbers damageNumbers;

        GameObject CurrentEnemy;

        // === Internal ===
        Health _enemyHealth;

        // === Public ===
        public static Health CurrentEnemyHealth { get { return Instance._enemyHealth; } }

        public static bool IsEnemyAvailable { get { return Instance.CurrentEnemy != null; } }

        void Awake()
        {
            Instance = this;

            StatsCache.Clear();

            Events.OnBossSpawned.AddListener(OnBossSpawned);
            Events.OnFailedToKillBoss.AddListener(OnFailedToKillBoss);
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
                if (!CurrentEnemyHealth.IsDead)
                {
                    Color col = StatsCache.ApplyCritHit(ref amount) ? Color.red : Color.white;

                    Instance.damageNumbers.Add(amount, col);

                    CurrentEnemyHealth.TakeDamage(amount);

                    Events.OnEnemyHurt.Invoke(CurrentEnemyHealth);

                    if (CurrentEnemyHealth.IsDead)
                    {
                        Instance.OnEnemyDeath();

                        Destroy(Instance.CurrentEnemy);
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

            _enemyHealth = CurrentEnemy.GetComponent<Health>();
        }

        void OnEnemyDeath()
        {
            if (Instance.CurrentEnemy.TryGetComponent(out LootDrop loot))
                loot.Process();

            // ===

            if (CurrentEnemy.CompareTag("Enemy"))
            {
                GameState.Stage.AddKill();

                Events.OnKillEnemy.Invoke();
            }

            else if (CurrentEnemy.CompareTag("BossEnemy"))
            {
                GameState.Stage.AdvanceStage();

                Events.OnNewStageStarted.Invoke();
            }

            SpawnNextEnemy();
        }

        void SpawnNextEnemy()
        {
            StartCoroutine(ISpawnNextEnemy());
        }

        IEnumerator ISpawnNextEnemy()
        {
            yield return new WaitForSeconds(Formulas.StageEnemy.SpawnDelay);

            if (!BossBattleManager.IsAvoidingBoss && GameState.Stage.isStageCompleted)
            {
                BossBattleManager.StartBossBattle();
            }
            else
            {
                Events.OnStageUpdate.Invoke();

                SpawnEnemy();
            }
        }

        void SpawnEnemy()
        {
            CurrentEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity, SpawnPoint);

            _enemyHealth = CurrentEnemy.GetComponent<Health>();

            Events.OnEnemySpawned.Invoke();
        }
    }
}