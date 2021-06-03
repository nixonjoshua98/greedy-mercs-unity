using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [SerializeField] Transform SpawnPoint;
        [Space]
        [SerializeField] GameObject[] EnemyObjects;
        [Space]
        [SerializeField] DamageNumbers damageNumbers;

        [Header("Controllers")]
        [SerializeField] StageBossController bossSpawner;

        GameObject currentEnemy;
        Health enemyHealth;

        public static Health CurrentEnemyHealth { get { return Instance.enemyHealth; } }

        public bool IsAllStageEnemiesKilled { get { return GameState.Stage.isStageCompleted; } }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            SubscribeToEvents();

            Invoke("SpawnNextEnemy", 0.5f);
        }

        void SubscribeToEvents()
        {
            bossSpawner.OnBossSpawn.AddListener(OnBossSpawn);
        }

        public static void TryDealDamageToEnemy(BigDouble amount)
        {
            if (Instance.currentEnemy != null)
            {
                if (!CurrentEnemyHealth.IsDead)
                {
                    Color col = StatsCache.ApplyCritHit(ref amount) ? Color.red : Color.white;

                    Instance.damageNumbers.Add(amount, col);

                    CurrentEnemyHealth.TakeDamage(amount);

                    GlobalEvents.OnEnemyHurt.Invoke(CurrentEnemyHealth);
                }
            }
        }

        // Called once ANY enemy has been killed 
        void OnAfterEnemyDeath()
        {
            Destroy(currentEnemy);

            SpawnNextEnemy();
        }

        // Event
        public void OnFailedToKillBoss()
        {
            SpawnNextEnemy();
        }

        // Event
        void OnBossSpawn(GameObject boss)
        {
            if (currentEnemy)
                Destroy(currentEnemy);

            currentEnemy = boss;

            enemyHealth = currentEnemy.GetComponent<Health>();

            enemyHealth.OnDeath.AddListener((_) => { OnBossEnemyDeath(); });
        }

        // Event
        void OnEnemyDeath()
        {
            if (Instance.currentEnemy.TryGetComponent(out LootDrop loot))
            {
                loot.Process();
            }
        }

        // Event
        void OnNormalEnemyDeath()
        {
            OnEnemyDeath();

            GameState.Stage.AddKill();

            GlobalEvents.OnKillEnemy.Invoke();

            OnAfterEnemyDeath();
        }

        void OnBossEnemyDeath()
        {
            OnEnemyDeath();

            GameState.Stage.AdvanceStage();

            GlobalEvents.OnNewStageStarted.Invoke();

            OnAfterEnemyDeath();
        }

        void SpawnNextEnemy()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return new WaitForSeconds(Formulas.StageEnemy.SpawnDelay);

                bool isAvoidingBoss = bossSpawner.isAvoidingBoss;

                if (!isAvoidingBoss && GameState.Stage.isStageCompleted)
                {
                    bossSpawner.Spawn();
                }

                else
                {
                    GlobalEvents.OnStageUpdate.Invoke();

                    SpawnEnemy();
                }
            }

            StartCoroutine(ISpawnNextEnemy());
        }

        void SpawnEnemy()
        {
            currentEnemy = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity, SpawnPoint);

            enemyHealth = currentEnemy.GetComponent<Health>();

            enemyHealth.OnDeath.AddListener((_) => { OnNormalEnemyDeath(); });

            GlobalEvents.OnEnemySpawned.Invoke();
        }
    }
}