using GM.Common;
using GM.States;
using System.Collections.Generic;
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

        [HideInInspector] public UnityEvent<GM.Units.UnitBaseClass> E_BossSpawn { get; private set; } = new UnityEvent<GM.Units.UnitBaseClass>();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveSpawn { get; private set; } = new UnityEvent<List<Units.UnitBaseClass>>();

        public List<GM.Units.UnitBaseClass> Enemies { get; private set; } = new List<Units.UnitBaseClass>();

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

        GameObject InstantiateBoss()
        {
            GameObject enemy = spawner.SpawnBoss();

            // Components
            GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            // Add event callbacks
            health.OnZeroHealth.AddListener(OnBossZeroHealth);

            Enemies.Add(unitClass);

            // Invoke an event
            E_BossSpawn.Invoke(unitClass);

            return enemy;
        }

        void SetupWave()
        {
            GameObject enemy = UnitManager.InstantiateEnemyUnit();

            // Components
            GM.Units.UnitBaseClass unitClass = enemy.GetComponent<GM.Units.UnitBaseClass>();

            Enemies.Add(unitClass);

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (GM.Units.UnitBaseClass trgt in Enemies)
            {
                GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();

                health.Invulnerable = true;

                health.Init(val: combinedHealth / Enemies.Count);

                health.OnZeroHealth.AddListener(() => OnEnemyZeroHealth(trgt));

                // Only allow the unit to be attacked once it is visible on screen
                StartCoroutine(Enumerators.InvokeAfter(() => Camera.main.IsVisible(trgt.transform.position), () => health.Invulnerable = false));
            }

            E_OnWaveSpawn.Invoke(Enemies);
        }

        void StartBossFight()
        {
            GameObject enemy = InstantiateBoss();

            // Update the state
            State.IsBossSpawned = true;

            // Set the boss position off-screen
            enemy.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);
        }


        // = ITargetManager = //

        public bool TryGetMercTarget(ref GM.Units.UnitBaseClass target)
        {
            target = default;

            if (Enemies.Count > 0)
                target = Enemies[0];

            return target == default;
        }

        // = Event Callbacks = //

        void OnEnemyZeroHealth(GM.Units.UnitBaseClass trgt)
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