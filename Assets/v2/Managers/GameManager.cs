using GM.Common;
using GM.States;
using GM.Targets;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour, ITargetManager
    {
        public static GameManager Instance { get; set; } = null;

        [Header("Controllers")]
        [SerializeField] SpawnController spawner;

        GM.Common.Interfaces.IUnitManager UnitManager;

        [HideInInspector] public UnityEvent<Target> E_BossSpawn { get; private set; } = new UnityEvent<Target>();
        [HideInInspector] public UnityEvent<TargetList<Target>> E_OnWaveSpawn { get; private set; } = new UnityEvent<TargetList<Target>>();

        public TargetList<Target> Enemies { get; private set; } = new TargetList<Target>();

        // = Quick Reference Properties = //
        GameState State => App.Data.GameState;

        void Awake()
        {
            Instance = this;

            UnitManager = this.GetComponentInScene<GM.Common.Interfaces.IUnitManager>();

            InitialSetup();
        }

        void Start()
        {
            StartWaveSystem();
        }

        void InitialSetup()
        {
            if (State.PreviouslyPrestiged)
            {
                State.PreviouslyPrestiged = false;

                App.SaveManager.Paused = false;
            }
        }

        void StartWaveSystem()
        {
            // Spawn the boss if the user has previously beaten the stage and reached the boss
            if (State.Wave == Constants.WAVES_PER_STAGE && State.IsBossSpawned)
            {
                StartBossFight();
            }
            else
            {
                SetupWave();
            }
        }


        public bool TryGetEnemy(out Target target)
        {
            return Enemies.TryGet(out target);
        }

        Target InstantiateBoss()
        {
            Target boss = new Target(spawner.SpawnBoss(), TargetType.Boss);

            // Setup the boss health
            boss.Health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            // Add event callbacks
            boss.Health.OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(boss);

            // Invoke an event
            E_BossSpawn.Invoke(boss);

            return boss;
        }

        void SetupWave()
        {
            Enemies.Add(new Target(spawner.InstantiateEnemyUnit(), TargetType.WaveEnemy));

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (Target trgt in Enemies)
            {
                trgt.Health.Invulnerable = true;

                trgt.Health.Init(val: combinedHealth / Enemies.Count);

                trgt.Health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));

                // Only allow the unit to be attacked once it is visible on screen
                StartCoroutine(Enumerators.InvokeAfter(() => Camera.main.IsVisible(trgt.Position), () => trgt.Health.Invulnerable = false));
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

        // Quick reference to avoid StartCorountines everywhere
        void StartBossFight()
        {
            Target boss = InstantiateBoss();

            // Update the state
            State.IsBossSpawned = true;

            // Set the boss position off-screen
            boss.GameObject.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);
        }


        // = ITargetManager = //

        public bool TryGetMercTarget(ref Target target)
        {
            return TryGetEnemy(out target);
        }

        // = Event Callbacks = //

        void OnEnemyZeroHealth(Target trgt)
        {
            Enemies.Remove(trgt);

            // All wave enemies have been defeated
            if (Enemies.Count == 0)
            {
                // Time to setup the boss fight
                if (App.Data.GameState.Wave == Constants.WAVES_PER_STAGE)
                {
                    StartBossFight();
                }

                else
                {
                    SetupWave();

                    App.Data.GameState.Wave++;
                }
            }
        }

        void OnBossZeroHealth()
        {
            Enemies.Clear(); // Clear all enemies (should only be 1)

            // Update the state, mainly used for saving and loading state
            State.IsBossSpawned = false;

            // Advance the stage and reset the wave
            State.Stage++;
            State.Wave = 1;

            // Setup the next wave
            SetupWave();
        }
    }
}