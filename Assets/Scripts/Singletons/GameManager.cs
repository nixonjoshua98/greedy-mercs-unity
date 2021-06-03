using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [SerializeField] Transform SpawnPoint;
        [Space]
        [SerializeField] GameObject[] EnemyObjects;
        [Space]
        [SerializeField] DamageNumbers damageNumbers;

        GameObject currentEnemy;
        Health enemyHealth;   

        public static Health CurrentEnemyHealth { get { return Instance.enemyHealth; } }

        public bool IsEnemyAvailable { get { return Instance.currentEnemy != null; } }
        public bool IsAllStageEnemiesKilled { get { return GameState.Stage.isStageCompleted; } }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            SubscribeToEvents();

            SpawnNextEnemy();
        }

        void SubscribeToEvents()
        {
            BossBattleManager.Instance.OnBossSpawn.AddListener(OnBossSpawn);

            BossBattleManager.Instance.OnFailedToKillBoss.AddListener(OnFailedToKillBoss);
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

                    Events.OnEnemyHurt.Invoke(CurrentEnemyHealth);
                }
            }
        }

        public void OnFailedToKillBoss()
        {
            SpawnNextEnemy();
        }

        void OnBossSpawn(GameObject boss)
        {
            if (currentEnemy)
                Destroy(currentEnemy);

            currentEnemy = boss;

            enemyHealth = currentEnemy.GetComponent<Health>();

            enemyHealth.OnDeath.AddListener((_) => { OnBossEnemyDeath(); });
        }

        void OnEnemyDeath()
        {
            if (Instance.currentEnemy.TryGetComponent(out LootDrop loot))
            {
                loot.Process();
            }
        }

        void OnAfterEnemyDeath()
        {
            Destroy(currentEnemy);

            SpawnNextEnemy();
        }

        void OnNormalEnemyDeath()
        {
            OnEnemyDeath();

            GameState.Stage.AddKill();

            Events.OnKillEnemy.Invoke();

            OnAfterEnemyDeath();
        }

        void OnBossEnemyDeath()
        {
            OnEnemyDeath();

            GameState.Stage.AdvanceStage();

            Events.OnNewStageStarted.Invoke();

            OnAfterEnemyDeath();
        }

        void SpawnNextEnemy()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return new WaitForSeconds(Formulas.StageEnemy.SpawnDelay);

                bool isAvoidingBoss = BossBattleManager.Instance.isAvoidingBoss;

                if (!isAvoidingBoss && GameState.Stage.isStageCompleted)
                {
                    BossBattleManager.StartBossBattle();
                }

                else
                {
                    Events.OnStageUpdate.Invoke();

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

            Events.OnEnemySpawned.Invoke();
        }
    }
}