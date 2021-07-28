using System.Collections;
using System.Collections.Generic;

using UnityEngine;



namespace GM
{
    public class CurrentStageState
    {
        public int Stage = 1;

        public CurrentStageState Copy()
        {
            return new CurrentStageState()
            {
                Stage = Stage
            };
        }
    }


    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController bossSpawner;

        public CurrentStageState state;

        List<GameObject> enemiesRemaining;

        void Awake()
        {
            Instance = this;

            state = new CurrentStageState();
            enemiesRemaining = new List<GameObject>();
        }


        void Start()
        {
            SpawnWave();
        }


        // = = = Public Methods = = = //
        public CurrentStageState State() => state.Copy();
        // = = = ^ //


        void SpawnWave()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return SpawnDelay();

                enemiesRemaining = bossSpawner.SpawnWave();

                BigDouble combinedStageHealth = Formulas.EnemyHealth(state.Stage);

                foreach (GameObject o in enemiesRemaining)
                {
                    HealthController hp = o.GetComponent<HealthController>();

                    hp.Setup(combinedStageHealth / enemiesRemaining.Count);

                    hp.E_OnZeroHealth.AddListener(() => {
                        OnEnemyZeroHealth(o);
                    });
                }
            }

            StartCoroutine(ISpawnNextEnemy());
        }


        void SpawnBoss()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return SpawnDelay();

                // Spawn the object
                GameObject boss = bossSpawner.SpawnBoss(state);

                // Grab components
                HealthController hp = boss.GetComponent<HealthController>();

                hp.Setup(val: Formulas.BossHealth(state.Stage));

                hp.E_OnZeroHealth.AddListener(() => { 
                    OnBossZeroHealth(boss); 
                });
            }

            StartCoroutine(ISpawnNextEnemy());
        }


        // = = = Events = = = //
        void OnEnemyZeroHealth(GameObject obj)
        {
            enemiesRemaining.Remove(obj);

            if (enemiesRemaining.Count == 0)
            {
                OnWaveCleared();
            }
        }


        void OnBossZeroHealth(GameObject boss)
        {
            state.Stage++;

            SpawnWave();
        }


        void OnWaveCleared()
        {
            state.Stage++;

            if (state.Stage % 5 == 0)
                SpawnBoss();

            else
                SpawnWave();
        }
        // = = = ^ //


        IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(0.25f);
        }
    }
}