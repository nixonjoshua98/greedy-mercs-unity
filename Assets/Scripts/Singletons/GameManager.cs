using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using GM.Targets;
using HealthController = GM.Controllers.HealthController;
using DamageClickController = GM.Controllers.DamageClickController;

namespace GM
{
    public class CurrentStageState
    {
        public int Stage = 1;
        public int Wave = 1;
        public int WavesPerStage = 1;

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

        [HideInInspector] public UnityEvent<Target> E_BossSpawn { get; private set; } = new UnityEvent<Target>();
        [HideInInspector] public UnityEvent E_OnWaveCleared { get; private set; } = new UnityEvent();
        [HideInInspector] public UnityEvent<TargetList> E_OnWaveSpawn { get; private set; } = new UnityEvent<TargetList>();

        public DamageClickController ClickController;

        // Contains the wave enemies, but can also contain the stage-end boss
        // Enemies.TryGetWithType(TargetType.Boss) can be used to fetch the boss
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
            if (Enemies.Count > 0)
            {
                Target target = Enemies.OrderBy(t => t.Health.Current).First();

                BigDouble dmg = App.Cache.TapDamage;

                target.Health.TakeDamage(dmg);
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

                trgt.Health.E_OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));

                healthControllers.Add(trgt.Health);
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

        void SpawnBoss()
        {
            Target boss = Enemies.Add(spawner.SpawnBoss(state), TargetType.Boss);

            boss.Health.Setup(val: App.Cache.StageBossHealthAtStage(state.Stage));

            boss.Health.E_OnZeroHealth.AddListener(OnBossZeroHealth);

            E_BossSpawn.Invoke(boss);
        }

        void OnEnemyZeroHealth(Target trgt)
        {
            Enemies.Remove(trgt);

            if (Enemies.Count == 0)
            {
                OnWaveCleared();
            }
        }

        void OnBossZeroHealth()
        {
            Enemies.Clear();

            state.Stage++;
            state.Wave = 1;

            SpawnWave();
        }

        void OnWaveCleared()
        {
            E_OnWaveCleared.Invoke();

            if (state.Wave == state.WavesPerStage)
            {
                SpawnBoss();
            }

            else
            {
                SpawnWave();

                state.Wave++;
            }
        }
    }
}