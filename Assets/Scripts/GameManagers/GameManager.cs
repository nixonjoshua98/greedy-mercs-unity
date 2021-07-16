using System.Collections;
using System.Collections.Generic;

using UnityEngine;



namespace GM
{
    using GM.Events;

    public class CurrentStageState
    {
        public int currentStage = 1;

        public CurrentStageState Copy()
        {
            return new CurrentStageState()
            {
                currentStage = currentStage
            };
        }
    }


    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] EnemySpawnController enemySpawner;
        [SerializeField] BossSpawnController bossSpawner;

        public CurrentStageState state;

        List<GameObject> spawnedEnemies;

        void Awake()
        {
            Instance = this;

            state           = new CurrentStageState();
            spawnedEnemies  = new List<GameObject>();
        }

        void Start()
        {
            SpawnEnemyWave();
        }

        // We create a copy to avoid accidental changes
        public CurrentStageState State()
        {
            return state.Copy();
        }

        void ProcessEnemyDeath(GameObject obj)
        {
            if (obj.TryGetComponent(out LootDrop loot))
            {
                loot.Process();
            }

            spawnedEnemies.Remove(obj);
        }

        void OnEnemyZeroHealth(GameObject obj)
        {
            ProcessEnemyDeath(obj);

            if (spawnedEnemies.Count == 0)
            {
                OnWaveCleared();

                GlobalEvents.E_OnWaveClear.Invoke(state.Copy());
            }
        }

        void OnBossZeroHealth(GameObject obj)
        {
            ProcessEnemyDeath(obj);

            state.currentStage++;

            SpawnEnemyWave();
        }

        void OnWaveCleared()
        {
            SpawnBoss();
        }

        void SpawnEnemyWave()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return new WaitForSeconds(0.25f);

                spawnedEnemies = enemySpawner.SpawnMultiple(3);

                foreach (GameObject o in spawnedEnemies)
                {
                    // Grab components
                    HealthController hp = o.GetComponent<HealthController>();

                    hp.E_OnDeath.AddListener(() => { OnEnemyZeroHealth(o); });
                }
            }

            StartCoroutine(ISpawnNextEnemy());
        }

        void SpawnBoss()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return new WaitForSeconds(0.25f);

                // Spawn the object
                GameObject o = bossSpawner.Spawn();

                // Grab components
                HealthController hp = o.GetComponent<HealthController>();

                // Events
                hp.E_OnDeath.AddListener(() => { OnBossZeroHealth(o); });

                // Setup
                spawnedEnemies.Add(o);
            }

            StartCoroutine(ISpawnNextEnemy());
        }
    }
}