using GM.Common;
using GM.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM
{
    public class GameManager : Core.GMMonoBehaviour
    {
        public static GameManager Instance { get; set; } = null;

        [Header("Controllers")]

        GM.Common.Interfaces.IUnitManager UnitManager;

        [HideInInspector] public UnityEvent<GM.Units.UnitBaseClass> E_BossSpawn { get; private set; } = new UnityEvent<GM.Units.UnitBaseClass>();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveSpawn { get; private set; } = new UnityEvent<List<Units.UnitBaseClass>>();

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
            GameObject enemy = UnitManager.InstantiateEnemyUnit();

            // Components
            GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.Cache.StageBossHealthAtStage(State.Stage));

            // Add event callbacks
            health.OnZeroHealth.AddListener(OnBossZeroHealth);

            // Invoke an event
            E_BossSpawn.Invoke(unitClass);

            return enemy;
        }

        void SetupWave()
        {
            List<GM.Units.UnitBaseClass> enemies = new List<Units.UnitBaseClass>();

            for (int i = 0; i < GM.Common.Constants.WAVES_PER_STAGE; i++)
            {
                enemies.Add(UnitManager.InstantiateEnemyUnit().GetComponent<Units.UnitBaseClass>());
            }

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(State.Stage);

            foreach (GM.Units.UnitBaseClass unit in enemies)
            {
                GM.Units.UnitBaseClass unitClass = unit.GetComponent<GM.Units.UnitBaseClass>();

                GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

                health.Init(combinedHealth);

                health.OnZeroHealth.AddListener(OnEnemyZeroHealth);
            }

            E_OnWaveSpawn.Invoke(enemies);
        }

        void StartBossFight()
        {
            GameObject enemy = InstantiateBoss();

            // Update the state
            State.IsBossSpawned = true;

            // Set the boss position off-screen
            enemy.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);
        }


        // = Event Callbacks = //

        void OnEnemyZeroHealth()
        {
            // All wave enemies have been defeated
            if (UnitManager.NumEnemyUnits == 0)
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