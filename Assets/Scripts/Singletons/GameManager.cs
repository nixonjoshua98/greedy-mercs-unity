using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    using GM.Events;

    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 1;

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

    public class GameManager : Core.GMMonoBehaviour
    {
        public static GameManager Instance = null;

        [Header("Controllers")]
        [SerializeField] SpawnController spawner;

        public CurrentStageState state;

        // Events
        public UnityEvent<Target> E_BossSpawn = new UnityEvent<Target>();
        public UnityEvent E_BossDeath = new UnityEvent();
        [HideInInspector] public UnityEvent E_OnWaveCleared;
        [HideInInspector] public UnityEvent<WaveSpawnEventData> E_OnWaveSpawn;

        public DamageClickController ClickController;
        public TargetList Enemies { get; private set; } = new TargetList();

        void Awake()
        {
            Instance = this;

            state = new CurrentStageState();

            ClickController.E_OnClick.AddListener(OnDamageClick);
        }


        void Start()
        {
            SpawnWave();
        }


        void OnDamageClick(Vector2 worldSpaceClickPosition)
        {
            if (Enemies.Count >= 1)
            {
                Target target = Enemies[0];

                Debug.Log($"Click attacked {target.GameObject.name} {worldSpaceClickPosition}");
            }
        }

        public CurrentStageState State() => state.Copy();

        void SpawnWave()
        {
            Enemies.AddRange(spawner.SpawnWave(), TargetType.WaveEnemy);

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(state.Stage);

            List<HealthController> healthControllers = new List<HealthController>();

            foreach (Target trgt in Enemies)
            {
                trgt.Health.Setup(val: combinedHealth / Enemies.Count);

                trgt.Health.E_OnZeroHealth.AddListener(() => {
                    OnEnemyZeroHealth(trgt.GameObject);
                });

                healthControllers.Add(trgt.Health);
            }

            E_OnWaveSpawn.Invoke(new WaveSpawnEventData()
            {
                CombinedHealth = combinedHealth,
                HealthControllers = healthControllers
            });
        }


        void SpawnBoss()
        {
            Target boss = Enemies.Add(spawner.SpawnBoss(state), TargetType.Boss);

            boss.Health.Setup(val: App.Cache.StageBossHealthAtStage(state.Stage));

            boss.Health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            OnBossSpawn();

            E_BossSpawn.Invoke(boss);
        }


        // = = = Events = = = //
        void OnBossSpawn()
        {
            state.IsBossAvailable = true;
        }

        /// <summary>
        /// Called once a wave enemy has their health reduced to 0
        /// </summary>
        void OnEnemyZeroHealth(GameObject obj)
        {
            Enemies.Remove(obj);

            if (Enemies.Count == 0)
            {
                OnWaveCleared();
            }
        }

        void OnBossZeroHealth()
        {
            Enemies.Clear();

            E_BossDeath.Invoke();

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
    }
}