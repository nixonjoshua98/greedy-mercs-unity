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

        IUnitManager UnitManager;

        [HideInInspector] public UnityEvent<GM.Units.UnitBaseClass> E_BossSpawn { get; private set; } = new UnityEvent<GM.Units.UnitBaseClass>();
        [HideInInspector] public UnityEvent<List<GM.Units.UnitBaseClass>> E_OnWaveSpawn { get; private set; } = new UnityEvent<List<Units.UnitBaseClass>>();

        // = Quick Reference Properties = //
        GameState CurrentGameState => App.Data.GameState;

        void Awake()
        {
            Instance = this;

            UnitManager = this.GetComponentInScene<IUnitManager>();

            InitialSetup();
        }

        void Start()
        {
            // Spawn the boss if the user has previously beaten the stage and reached the boss
            if (CurrentGameState.EnemiesRemaining == 0)
            {
                StartStageBoss();
            }
            else
            {
                SetupWave();
            }
        }


        public void DealDamageToTarget(BigDouble damageValue)
        {
            if (UnitManager.NumEnemyUnits == 0)
                return;

            GM.Units.UnitBaseClass unit = UnitManager.GetNextEnemyUnit();

            GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

            health.TakeDamage(damageValue);
        }


        void InitialSetup()
        {
            if (CurrentGameState.PreviouslyPrestiged)
            {
                CurrentGameState.PreviouslyPrestiged = false;

                App.SaveManager.Paused = false;
            }
        }

        void SetupWave()
        {
            List<GM.Units.UnitBaseClass> enemies = new List<Units.UnitBaseClass>();

            for (int i = 0; i < CurrentGameState.EnemiesRemaining; i++)
            {
                enemies.Add(UnitManager.InstantiateEnemyUnit().GetComponent<Units.UnitBaseClass>());
            }

            BigDouble combinedHealth = App.Cache.EnemyHealthAtStage(CurrentGameState.Stage);

            foreach (GM.Units.UnitBaseClass unit in enemies)
            {
                GM.Units.UnitBaseClass unitClass = unit.GetComponent<GM.Units.UnitBaseClass>();

                GM.Controllers.HealthController health = unit.GetComponent<GM.Controllers.HealthController>();

                health.Init(combinedHealth);

                health.OnZeroHealth.AddListener(OnEnemyZeroHealth);
            }

            E_OnWaveSpawn.Invoke(enemies);
        }

        void StartStageBoss()
        {
            GameObject enemy = UnitManager.InstantiateEnemyUnit();

            // Components
            GM.Controllers.HealthController health = enemy.GetComponent<GM.Controllers.HealthController>();
            GM.Units.UnitBaseClass unitClass = enemy.GetComponent<GM.Units.UnitBaseClass>();

            // Setup
            health.Init(val: App.Cache.StageBossHealthAtStage(CurrentGameState.Stage));

            // Add event callbacks
            health.OnZeroHealth.AddListener(OnBossZeroHealth);

            // Update the state
            CurrentGameState.HasBossSpawned = true;

            // Set the boss position off-screen
            enemy.transform.position = new Vector3(Camera.main.MaxBounds().x + 2.5f, Constants.CENTER_BATTLE_Y);

            // Invoke an event
            E_BossSpawn.Invoke(unitClass);
        }


        // = Event Callbacks = //

        void OnEnemyZeroHealth()
        {
            CurrentGameState.EnemiesDefeated++;

            // All wave enemies have been defeated
            if (UnitManager.NumEnemyUnits == 0)
            {
                StartStageBoss();
            }
        }

        void OnBossZeroHealth()
        {
            // Update the state, mainly used for saving and loading state
            CurrentGameState.HasBossSpawned = false;

            CurrentGameState.Stage++;
            CurrentGameState.EnemiesDefeated = 0;

            // Setup the next wave
            SetupWave();
        }
    }
}