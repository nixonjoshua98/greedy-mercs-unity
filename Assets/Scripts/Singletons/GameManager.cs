using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace GM
{
    using GM.Events;

    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 3;

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
        [SerializeField] SpawnController spawner;

        public CurrentStageState state;

        public List<GameObject> WaveEnemies { get; private set; }
        GameObject CurrentBossEenemy;

        // Events
        public UnityEvent<GameObject> E_BossSpawn = new UnityEvent<GameObject>();
        public UnityEvent E_BossDeath = new UnityEvent();
        [HideInInspector] public UnityEvent E_OnWaveCleared;
        [HideInInspector] public UnityEvent<WaveSpawnEventData> E_OnWaveSpawn;

        public bool TryGetWaveEnemy(out GameObject enemy)
        {
            enemy = null;

            foreach (GameObject obj in WaveEnemies.OrderBy(x => Random.value))
            {
                if (obj.TryGetComponent(out HealthController health) && health.CurrentHealth > 0)
                {
                    enemy = obj;

                    return true;
                }
            }

            return false;
        }

        void Awake()
        {
            Instance = this;

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

                WaveEnemies = spawner.SpawnWave();

                BigDouble combinedHealth = Formulas.EnemyHealth(state.Stage);

                List<HealthController> healthControllers = new List<HealthController>();

                foreach (GameObject o in WaveEnemies)
                {
                    HealthController hp = o.GetComponent<HealthController>();

                    hp.Setup(combinedHealth / WaveEnemies.Count);

                    hp.E_OnZeroHealth.AddListener(() => {
                        OnEnemyZeroHealth(o);
                    });

                    healthControllers.Add(hp);
                }

                E_OnWaveSpawn.Invoke(new WaveSpawnEventData()
                {
                    Enemies = WaveEnemies,
                    CombinedHealth = combinedHealth,
                    HealthControllers = healthControllers
                });
            }

            StartCoroutine(ISpawnNextEnemy());
        }


        void SpawnBoss()
        {
            IEnumerator ISpawnNextEnemy()
            {
                yield return SpawnDelay();

                // Spawn the object
                CurrentBossEenemy = spawner.SpawnBoss(state);

                // Grab components
                HealthController hp = CurrentBossEenemy.GetComponent<HealthController>();

                hp.Setup(val: Formulas.BossHealth(state.Stage));

                hp.E_OnZeroHealth.AddListener(OnBossZeroHealth);

                OnBossSpawn();

                E_BossSpawn.Invoke(CurrentBossEenemy);
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
            E_BossDeath.Invoke();
            CurrentBossEenemy = null;

            state.Stage++;
            state.Wave = 1;
            state.IsBossAvailable = false;

            SpawnWave();
        }


        void OnWaveCleared()
        {
            E_OnWaveCleared.Invoke();

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