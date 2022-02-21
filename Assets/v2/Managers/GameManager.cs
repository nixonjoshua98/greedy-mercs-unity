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

        Target InstantiateBoss()
        {
            GameObject enemy = spawner.SpawnBoss();

            Target boss = new Target(enemy);

            // Components
            GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();

            // Setup
            health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            // Add event callbacks
            health.OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(boss);

            // Invoke an event
            E_BossSpawn.Invoke(boss);

            return boss;
        }

        void SetupWave()
        {
            GameObject enemy = UnitManager.InstantiateEnemyUnit();

            Enemies.Add(new Target(enemy));

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (Target trgt in Enemies)
            {
                GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();

                health.Invulnerable = true;

                health.Init(val: combinedHealth / Enemies.Count);

                health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));

                // Only allow the unit to be attacked once it is visible on screen
                StartCoroutine(Enumerators.InvokeAfter(() => Camera.main.IsVisible(trgt.Position), () => health.Invulnerable = false));
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

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
            return Enemies.TryGet(out target);
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