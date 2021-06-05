using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * 
 */

namespace GM
{
    using GM.Events;

    public class C_GameState
    {
        public int currentStage = 1;

        public C_GameState Copy()
        {
            return new C_GameState()
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

        public C_GameState state;

        List<GameObject> spawnedEnemies;

        void Awake()
        {
            Instance = this;

            state           = new C_GameState();
            spawnedEnemies  = new List<GameObject>();
        }

        void Start()
        {
            SpawnEnemyWave();
        }

        // Public method used to get the current state
        // We create a copy to avoid accidental changes
        public C_GameState GetState()
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
                    AbstractHealthController hp = o.GetComponent<AbstractHealthController>();

                    hp.E_OnDeath.AddListener(OnEnemyZeroHealth);
                }
            }

            StartCoroutine(ISpawnNextEnemy());
        }

        void SpawnBoss()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return new WaitForSeconds(0.25f);

                GameObject o = bossSpawner.Spawn();

                AbstractHealthController hp = o.GetComponent<AbstractHealthController>();

                hp.E_OnDeath.AddListener(OnBossZeroHealth);

                spawnedEnemies.Add(o);
            }

            StartCoroutine(ISpawnNextEnemy());
        }
    }
}