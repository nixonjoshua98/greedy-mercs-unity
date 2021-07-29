using System.Collections;
using System.Collections.Generic;

using UnityEngine;



namespace GM
{
    using GM.Events;

    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 5;

        [System.NonSerialized]
        public bool IsBossAvailable = false;

        public CurrentStageState Copy()
        {
            return new CurrentStageState()
            {
                Stage = Stage,
                Wave = Wave,
                WavesPerStage = WavesPerStage
            };
        }
    }


    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController bossSpawner;

        public CurrentStageState state;

        public List<GameObject> WaveEnemies { get; private set; }
        GameObject CurrentBossEenemy;

        // Events
        public GameObjectEvent E_OnBossSpawn;

        void Awake()
        {
            Instance = this;

            E_OnBossSpawn = new GameObjectEvent();

            state = new CurrentStageState();
            WaveEnemies = new List<GameObject>();
        }


        void Start()
        {
            SpawnWave();
        }


        // = = = Public Methods = = = //
        public static GameManager Get => Instance;
        public CurrentStageState State() => state.Copy();

        public bool TryGetBoss(out GameObject boss)
        {
            boss = CurrentBossEenemy;

            return state.IsBossAvailable;
        }

        // = = = ^ //


        void SpawnWave()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return SpawnDelay();

                WaveEnemies = bossSpawner.SpawnWave();

                BigDouble combinedStageHealth = Formulas.EnemyHealth(state.Stage);

                foreach (GameObject o in WaveEnemies)
                {
                    HealthController hp = o.GetComponent<HealthController>();

                    hp.Setup(combinedStageHealth / WaveEnemies.Count);

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
                CurrentBossEenemy = bossSpawner.SpawnBoss(state);

                // Grab components
                HealthController hp = CurrentBossEenemy.GetComponent<HealthController>();

                hp.Setup(val: Formulas.BossHealth(state.Stage));

                hp.E_OnZeroHealth.AddListener(OnBossZeroHealth);

                OnBossSpawn();

                E_OnBossSpawn.Invoke(CurrentBossEenemy);
            }

            StartCoroutine(ISpawnNextEnemy());
        }


        // = = = Events = = = //
        void OnBossSpawn()
        {
            state.IsBossAvailable = true;
        }

        void OnEnemyZeroHealth(GameObject obj)
        {
            WaveEnemies.Remove(obj);

            if (WaveEnemies.Count == 0)
            {
                OnWaveCleared();
            }
        }


        void OnBossZeroHealth()
        {
            CurrentBossEenemy = null;

            state.Stage++;
            state.Wave = 1;
            state.IsBossAvailable = false;

            SpawnWave();
        }


        void OnWaveCleared()
        {
            if (state.Wave == state.WavesPerStage)
                SpawnBoss();

            else
            {
                SpawnWave();

                state.Wave++;
            }
        }
        // = = = ^ //


        IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(0.25f);
        }
    }
}