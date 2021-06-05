using System.Collections;

using UnityEngine;

namespace GM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] StageBossController bossSpawner;
        [SerializeField] EnemyWaveController enemySpawner;

        GameObject prevEnemySpawn;
        Health enemyHealth;

        public static Health CurrentEnemyHealth { get { return Instance.enemyHealth; } }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            SubscribeToEvents();

            Invoke("SpawnNextEnemy", 0.25f);
        }

        void SubscribeToEvents()
        {
            bossSpawner.OnBossSpawn.AddListener(OnBossSpawn);
        }

        // Called once ANY enemy has been killed 
        void OnAfterEnemyDeath()
        {
            SpawnNextEnemy();
        }

        // Event
        public void OnFailedToKillBoss()
        {
            SpawnNextEnemy();
        }

        // UnityEvent - Called from the Boss Controller/Spawner
        // Setup the callbacks required to continue the stage after death
        void OnBossSpawn(GameObject boss)
        {
            if (prevEnemySpawn)
                Destroy(prevEnemySpawn);

            prevEnemySpawn = boss;

            Health hp = boss.GetComponent<Health>();

            hp.OnDeath.AddListener(OnBossEnemyDeath);
        }

        // Event
        void OnEnemyDeath(GameObject obj)
        {
            if (obj.TryGetComponent(out LootDrop loot))
            {
                loot.Process();
            }
        }

        // Event
        void OnNormalEnemyDeath(GameObject obj)
        {
            OnEnemyDeath(obj);

            GameState.Stage.AddKill();

            OnAfterEnemyDeath();
        }

        // UnityEvent - Called internally based off Health.OnDeath
        void OnBossEnemyDeath(GameObject obj)
        {
            OnEnemyDeath(obj);

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
            prevEnemySpawn = enemySpawner.Spawn();

            enemyHealth = prevEnemySpawn.GetComponent<Health>();

            enemyHealth.OnDeath.AddListener(OnNormalEnemyDeath);
        }
    }
}